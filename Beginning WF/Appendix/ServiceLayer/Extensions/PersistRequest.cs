using System;
using System.Activities.Persistence;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Data.Linq;
using System.Transactions;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace ServiceLayer.Extensions
{
    /*****************************************************/
    // The extension class is used to define the behavior
    /*****************************************************/
    public class PersistRequestExtension : BehaviorExtensionElement
    {
        public PersistRequestExtension()
        {
            Console.WriteLine("Behavior extension started");
        }

        [ConfigurationProperty("connectionStringName", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                ConnectionStringSettingsCollection connectionStrings = WebConfigurationManager.ConnectionStrings;
                if (connectionStrings == null) return null;
                string connectionString = null;
                if (connectionStrings[ConnectionStringName] != null)
                {
                    connectionString = connectionStrings[ConnectionStringName].ConnectionString;
                }
                if (connectionString == null)
                {
                    throw new ConfigurationErrorsException("Connection string is required");
                }
                return connectionString;
            }
        }

        public override Type BehaviorType { get { return typeof(PersistRequestBehavior); } }
        protected override object CreateBehavior() { return new PersistRequestBehavior(ConnectionString); }
    }

    /*****************************************************/
    // The behavior class is used to create an exention for
    // each new instance
    /*****************************************************/
    public class PersistRequestBehavior : IServiceBehavior
    {
        string _connectionString;

        public PersistRequestBehavior(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            WorkflowServiceHost workflowServiceHost = serviceHostBase as WorkflowServiceHost;
            if (null != workflowServiceHost)
            {
                string workflowDisplayName = workflowServiceHost.Activity.DisplayName;

                workflowServiceHost.WorkflowExtensions.Add(()
                        => new PersistRequest(_connectionString));
            }
        }

        public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }

    /*****************************************************/
    // This is the actual extension class
    /*****************************************************/
    public class PersistRequest : PersistenceParticipant
    {
        private string _connectionString;
        private IDictionary<string, Request> _actions;

        public PersistRequest()
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            _connectionString = css.ConnectionStrings["Request"].ConnectionString;

            _actions = new Dictionary<string, Request>();
        }

        public PersistRequest(string connectionString)
        {
            _connectionString = connectionString;
            _actions = new Dictionary<string, Request>();
        }

        public PersistRequest(NameValueCollection parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters",
                    "The name value collection 'parameters' cannot be null");
            }
            string connectionString = parameters["ConnectionString"];
            if (connectionString != null)
            {
                _connectionString = connectionString;
            }
            else
            {
                throw new ArgumentNullException("ConnectionString",
                    "Connection string not found in the name-value collection");
            }
        }

        // Add an item to the work queue
        internal void AddRequest(Request r)
        {
            if (r.RequestID != 0)
            {
                _actions.Remove(r.RequestID.ToString());
                _actions.Add(r.RequestID.ToString(), r);
            }
            else
             _actions.Add("", r);
        }

        // Override this method to persist the work queue
        protected override void CollectValues
            (out IDictionary<XName, object> readWriteValues,
             out IDictionary<XName, object> writeOnlyValues)
        {
            // We're not actually providing data to the caller
            readWriteValues = null;
            writeOnlyValues = null;

            // See if there is any work to do...
            if (_actions.Count > 0)
            {
                // Get the current transaction
                Transaction t = System.Transactions.Transaction.Current;

                // Setup the DataContext
                RequestDataContext dc = new RequestDataContext(_connectionString);

                // Open the connection, if necessary
                if (dc.Connection.State == System.Data.ConnectionState.Closed)
                    dc.Connection.Open();

                if (t != null)
                    dc.Connection.EnlistTransaction(t);

                // Process each object in our work queue
                foreach (KeyValuePair<string, Request> kvp in _actions)
                {
                    Request r = kvp.Value as Request;

                    // Perform the insert
                    if (kvp.Key == "")
                    {
                        dc.Requests.InsertOnSubmit(r);
                    }

                    // Perform the update
                    else
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Requests);
                        Request rTmp = dc.Requests.SingleOrDefault<Request>
                            (x => x.RequestID == r.RequestID);

                        if (rTmp != null)
                        {
                            rTmp.ActionTaken = r.ActionTaken;
                            rTmp.RouteNext = r.RouteNext;
                        }
                    }
                }

                // Submit all the changes to the database
                dc.SubmitChanges();

                // Remove all objects since the changes have been submitted
                _actions.Clear();
            }
        }
    }
}