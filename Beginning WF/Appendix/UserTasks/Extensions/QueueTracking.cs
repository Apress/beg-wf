using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace UserTasks.Extensions
{
    /*****************************************************/
    // The extension class is used to define the behavior
    /*****************************************************/
    public class QueueTrackingExtension : BehaviorExtensionElement
    {
        public QueueTrackingExtension()
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

        public override Type BehaviorType { get { return typeof(QueueTrackingBehavior); } }
        protected override object CreateBehavior() { return new QueueTrackingBehavior(ConnectionString); }
    }

    /*****************************************************/
    // The behavior class is used to create an exention for
    // each new instance
    /*****************************************************/
    public class QueueTrackingBehavior : IServiceBehavior
    {
        string _connectionString;

        public QueueTrackingBehavior(string connectionString)
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
                        => new QueueTracking(_connectionString));
            }
        }

        public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }

    /*****************************************************/
    // This is the actual extension class
    /*****************************************************/
    public class QueueTracking : TrackingParticipant
    {
        private string _connectionString = "";

        public QueueTracking(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            CustomTrackingRecord customTrackingRecord =
                record as CustomTrackingRecord;

            if (customTrackingRecord != null)
            {
                if (customTrackingRecord.Name == "Start"    ||
                    customTrackingRecord.Name == "Route"    ||
                    customTrackingRecord.Name == "Assign"   ||
                    customTrackingRecord.Name == "UnAssign" ||
                    customTrackingRecord.Name == "QC")
                {
                    QueueTrack t = new QueueTrack();

                    // Extract all the user data 
                    if ((customTrackingRecord != null) &&
                        (customTrackingRecord.Data.Count > 0))
                    {
                        foreach (string key in customTrackingRecord.Data.Keys)
                        {
                            switch (key)
                            {
                                case "QueueInstanceKey":
                                    if (customTrackingRecord.Data[key] != null)
                                        t.QueueInstanceKey = (Guid)customTrackingRecord.Data[key];
                                    break;
                                case "SubQueueID":
                                    if (customTrackingRecord.Data[key] != null)
                                        t.SubQueueID = (int)customTrackingRecord.Data[key];
                                    break;
                                case "QC":
                                    if (customTrackingRecord.Data[key] != null)
                                        t.QC = (bool)customTrackingRecord.Data[key];
                                    break;
                                case "OperatorKey":
                                    if (customTrackingRecord.Data[key] != null)
                                        t.OperatorKey = (Guid)customTrackingRecord.Data[key];
                                    break;
                            }
                        }
                    }

                    if (t.SubQueueID != null && t.QC == null)
                        t.QC = false;

                    t.EventType = customTrackingRecord.Name;
                    t.EventDate = DateTime.UtcNow;

                    // Insert a record into the TrackUser table
                    UserTasksDataContext dc =
                        new UserTasksDataContext(_connectionString);
                    dc.QueueTracks.InsertOnSubmit(t);
                    dc.SubmitChanges();
                }
            }
        }
    }
}
