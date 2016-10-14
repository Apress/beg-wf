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
using System.Activities.Tracking;

namespace LeadGenerator
{
    /// <summary>
    /// Interaction logic for AddLead.xaml
    /// </summary>
    public partial class AddLead : Window
    {
        private string _connectionString = "";
        private InstanceStore _instanceStore;
        private DBExtension _dbExtension;
        private ListBoxTrackingParticipant _tracking;
        private EtwTrackingParticipant _etwTracking;
        private SqlTrackingParticipant _sqlTracking;

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

            // Create the DBExtension
            _dbExtension = new DBExtension(_connectionString);

            // Set up the tracking participants
            CreateTrackingParticipant();
            CreateETWTrackingParticipant();
            CreateSqlTrackingParticipant();

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
            parameters.Add("Rating", int.Parse(txtRating.Text));
            parameters.Add("Writer", new ListBoxTextWriter(lstEvents));

            WorkflowApplication i = new WorkflowApplication
                (new EnterLead(), parameters);

            // Setup persistence
            SetupInstance(i);

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
                
                SetupInstance(i); 
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

        private void SetupInstance(WorkflowApplication i)
        {
            // Setup the instance store
            i.InstanceStore = _instanceStore;

            // Setup the PersistableIdle event handler
            i.PersistableIdle = (waiea) => PersistableIdleAction.Unload;

            // Setup the connection string
            i.Extensions.Add(_dbExtension);

            // Display the accumulated comments
            i.Completed = (wacea) =>
            {
                // Get the CommentExtension
                IEnumerable<CommentExtension> q =
                    wacea.GetInstanceExtensions<CommentExtension>();

                // Add the comments to the event log
                if (q.Count() > 0)
                {
                    string comments = "Comments: \r\n" +
                        q.First<CommentExtension>().Comments;
                    this.lstEvents.Dispatcher.BeginInvoke
                        (new Action(() => this.lstEvents.Items.Add(comments)));
                }

                this.lstEvents.Dispatcher.BeginInvoke
                    (new Action(() => this.lstEvents.Items.Add
                        ("\r\nThe workflow has completed")));
            };

            // Set up tracking
            i.Extensions.Add(_tracking);
            i.Extensions.Add(_etwTracking);
            i.Extensions.Add(_sqlTracking);
        }

        private void CreateTrackingParticipant()
        {
            _tracking = new ListBoxTrackingParticipant(this.lstEvents)
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "ListBoxTrackingProfile",
                    Queries = 
                    {
                        // For instance data, only track the started and completed events
                        new WorkflowInstanceQuery()
                        {
                            States = { WorkflowInstanceStates.Started, 
                                       WorkflowInstanceStates.Completed },
                        },
                        
                        // For bookmark data, only track the GetAssignment event
                        new BookmarkResumptionQuery()
                        {
                            Name = "GetAssignment"
                        },
                        
                        // For activity data, track all states of the InvokeMethod
                        new ActivityStateQuery()
                        {
                            ActivityName = "InvokeMethod",
                            States = { "*" },
                        }, 
          
                        // For User data, track all events
                        new CustomTrackingQuery() 
                        {
                            Name = "*",
                            ActivityName = "*"
                        }
                    }
                }
            };
        }

        private void CreateETWTrackingParticipant()
        {
            _etwTracking = new EtwTrackingParticipant()
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "EtwTrackingProfile",
                    Queries = 
                    {
                        new CustomTrackingQuery() 
                        {
                         Name = "*",
                         ActivityName = "*"
                        }
                    }
                }
            };
        }

        private void CreateSqlTrackingParticipant()
        {
            _sqlTracking = new SqlTrackingParticipant(_connectionString)
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "SqlTrackingProfile",
                    Queries = 
                    {
                        new WorkflowInstanceQuery()
                        {
                            States = { "*" },
                        },
                        
                        new BookmarkResumptionQuery()
                        {
                            Name = "*"
                        },
                        new ActivityStateQuery()
                        {
                            // Subscribe for track records from all activities 
                            // for all states
                            ActivityName = "*",
                            States = { "*" },
                        },   
                        // For User data, track all events
                        new CustomTrackingQuery() 
                        {
         
                            Name = "*",
                            ActivityName = "*"
                        }
                    }
                }
            };
        }
    }
}
