using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Activities;
using System.Activities.DurableInstancing;
using System.Runtime.DurableInstancing;
using System.Data.Linq;
using System.Configuration;

namespace LeadGenerator
{
    /// <summary>
    /// Interaction logic for AddLead.xaml
    /// </summary>
    public partial class AddLead : Window
    {
        private string _connectionString = "";
        private InstanceStore _instanceStore;

        public AddLead()
        {
            InitializeComponent();

            ApplicationInterface._app = this;
        }

        // Add a line of text to the Event Log
        private void AddEvent(string szText)
        {
            lstEvents.Items.Add(szText);
        }

        public ListBox GetEventListBox()
        {
            return this.lstEvents;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Open the config file and get the connection string
            Configuration config = 
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection css = 
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            _connectionString = css.ConnectionStrings["LeadGenerator"].ConnectionString;

            _instanceStore = new SqlWorkflowInstanceStore(_connectionString);
            InstanceView view = _instanceStore.Execute
                (_instanceStore.CreateInstanceHandle(),
                new CreateWorkflowOwnerCommand(),
                TimeSpan.FromSeconds(30));
            _instanceStore.DefaultInstanceOwner = view.InstanceOwner;
        
            LoadExistingLeads();
        }

        private void btnAddLead_Click(object sender, RoutedEventArgs e)
        {
            // Setup a dictionary object for passing parameters
            Dictionary<string, object> parameters = 
                new Dictionary<string, object>();
            parameters.Add("ContactName", txtName.Text);
            parameters.Add("ContactPhone", txtPhone.Text);
            parameters.Add("Interests", txtInterest.Text);
            parameters.Add("Notes", txtNotes.Text);
            parameters.Add("ConnectionString", _connectionString);
            parameters.Add("Rating", int.Parse(txtRating.Text));
            parameters.Add("Writer", new ListBoxTextWriter(lstEvents));

            WorkflowApplication i = new WorkflowApplication
                (new EnterLead(), parameters);

            // Setup persistence
            i.InstanceStore = _instanceStore;
            i.PersistableIdle = (waiea) => PersistableIdleAction.Unload;

            i.Run();
        }

        public void AddNewLead(Lead l)
        {
            this.lstLeads.Dispatcher.BeginInvoke
                (new Action(() => this.lstLeads.Items.Add(l)));
        }

        private void lstLeads_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (lstLeads.SelectedIndex >= 0)
            {
                Lead l = (Lead)lstLeads.Items[lstLeads.SelectedIndex];
                lblSelectedNotes.Content = l.Comments;
                lblSelectedNotes.Visibility = Visibility.Visible;
                if (l.Status == "Open")
                {
                    lblAgent.Visibility = Visibility.Visible;
                    txtAgent.Visibility = Visibility.Visible;
                    btnAssign.Visibility = Visibility.Visible;
                }
                else
                {
                    lblAgent.Visibility = Visibility.Hidden;
                    txtAgent.Visibility = Visibility.Hidden;
                    btnAssign.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                lblSelectedNotes.Content = "";
                lblSelectedNotes.Visibility = Visibility.Hidden;
                lblAgent.Visibility = Visibility.Hidden;
                txtAgent.Visibility = Visibility.Hidden;
                btnAssign.Visibility = Visibility.Hidden;
            }
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            if (lstLeads.SelectedIndex >= 0)
            {
                Lead l = (Lead)lstLeads.Items[lstLeads.SelectedIndex];
                Guid id = l.WorkflowID;

                LeadDataDataContext dc = new LeadDataDataContext(_connectionString);
                dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Leads);
                l = dc.Leads.SingleOrDefault<Lead>(x => x.WorkflowID == id);
                if (l != null)
                {
                    l.AssignedTo = txtAgent.Text;
                    l.Status = "Assigned";
                    dc.SubmitChanges();

                    // Clear the input
                    txtAgent.Text = "";
                }

                // Update the grid
                lstLeads.Items[lstLeads.SelectedIndex] = l;
                lstLeads.Items.Refresh();

                WorkflowApplication i = new WorkflowApplication(new EnterLead());
                i.InstanceStore = _instanceStore;
                i.PersistableIdle = (waiea) => PersistableIdleAction.Unload;
                i.Load(id);

                try
                {
                    i.ResumeBookmark("GetAssignment", l);
                }
                catch (Exception e2)
                {
                    AddEvent(e2.Message);
                }
            }
        }

        private void LoadExistingLeads()
        {
            LeadDataDataContext dc = new LeadDataDataContext(_connectionString);
            dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Leads);
            IEnumerable<Lead> q = dc.Leads
                .Where<Lead>(x => x.Status == "Open" || x.Status == "Assigned");
            foreach (Lead l in q)
            {
                AddNewLead(l);
            }
        }
    }
}
