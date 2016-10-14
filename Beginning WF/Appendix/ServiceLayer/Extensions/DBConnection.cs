using System;
//using System.Activities.Persistence;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Data.Linq;
//using System.Transactions;
//using System.Xml.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace ServiceLayer.Extensions
{
    public class DBConnectionExtension : BehaviorExtensionElement
    {
        public DBConnectionExtension()
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

        public override Type BehaviorType { get { return typeof(DBConnectionBehavior); } }
        protected override object CreateBehavior() { return new DBConnectionBehavior(ConnectionString); }
    }

    public class DBConnectionBehavior : IServiceBehavior
    {
        string _connectionString;

        public DBConnectionBehavior(string connectionString)
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
                        => new DBConnection(_connectionString));
            }
        }

        public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }

    public class DBConnection
    {
        private string _connectionString = "";

        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }
    }
}
