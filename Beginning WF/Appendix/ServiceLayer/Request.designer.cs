﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceLayer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Appendix")]
	public partial class RequestDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertRequest(Request instance);
    partial void UpdateRequest(Request instance);
    partial void DeleteRequest(Request instance);
    partial void InsertQueueInstance(QueueInstance instance);
    partial void UpdateQueueInstance(QueueInstance instance);
    partial void DeleteQueueInstance(QueueInstance instance);
    #endregion
		
		public RequestDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["Request"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public RequestDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RequestDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RequestDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RequestDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Request> Requests
		{
			get
			{
				return this.GetTable<Request>();
			}
		}
		
		public System.Data.Linq.Table<QueueInstance> QueueInstances
		{
			get
			{
				return this.GetTable<QueueInstance>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Request")]
	public partial class Request : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _RequestID;
		
		private System.Guid _RequestKey;
		
		private string _RequestType;
		
		private string _UserName;
		
		private string _UserEmail;
		
		private string _Comment;
		
		private System.DateTime _CreateDate;
		
		private System.Guid _QueueInstanceKey;
		
		private string _ActionTaken;
		
		private string _RouteNext;
		
		private EntityRef<QueueInstance> _QueueInstance;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnRequestIDChanging(int value);
    partial void OnRequestIDChanged();
    partial void OnRequestKeyChanging(System.Guid value);
    partial void OnRequestKeyChanged();
    partial void OnRequestTypeChanging(string value);
    partial void OnRequestTypeChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnUserEmailChanging(string value);
    partial void OnUserEmailChanged();
    partial void OnCommentChanging(string value);
    partial void OnCommentChanged();
    partial void OnCreateDateChanging(System.DateTime value);
    partial void OnCreateDateChanged();
    partial void OnQueueInstanceKeyChanging(System.Guid value);
    partial void OnQueueInstanceKeyChanged();
    partial void OnActionTakenChanging(string value);
    partial void OnActionTakenChanged();
    partial void OnRouteNextChanging(string value);
    partial void OnRouteNextChanged();
    #endregion
		
		public Request()
		{
			this._QueueInstance = default(EntityRef<QueueInstance>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int RequestID
		{
			get
			{
				return this._RequestID;
			}
			set
			{
				if ((this._RequestID != value))
				{
					this.OnRequestIDChanging(value);
					this.SendPropertyChanging();
					this._RequestID = value;
					this.SendPropertyChanged("RequestID");
					this.OnRequestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestKey", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid RequestKey
		{
			get
			{
				return this._RequestKey;
			}
			set
			{
				if ((this._RequestKey != value))
				{
					this.OnRequestKeyChanging(value);
					this.SendPropertyChanging();
					this._RequestKey = value;
					this.SendPropertyChanged("RequestKey");
					this.OnRequestKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RequestType", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string RequestType
		{
			get
			{
				return this._RequestType;
			}
			set
			{
				if ((this._RequestType != value))
				{
					this.OnRequestTypeChanging(value);
					this.SendPropertyChanging();
					this._RequestType = value;
					this.SendPropertyChanged("RequestType");
					this.OnRequestTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="VarChar(100)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserEmail", DbType="VarChar(70)")]
		public string UserEmail
		{
			get
			{
				return this._UserEmail;
			}
			set
			{
				if ((this._UserEmail != value))
				{
					this.OnUserEmailChanging(value);
					this.SendPropertyChanging();
					this._UserEmail = value;
					this.SendPropertyChanged("UserEmail");
					this.OnUserEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Comment", DbType="VarChar(2000)")]
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if ((this._Comment != value))
				{
					this.OnCommentChanging(value);
					this.SendPropertyChanging();
					this._Comment = value;
					this.SendPropertyChanged("Comment");
					this.OnCommentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateDate", DbType="DateTime NOT NULL")]
		public System.DateTime CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				if ((this._CreateDate != value))
				{
					this.OnCreateDateChanging(value);
					this.SendPropertyChanging();
					this._CreateDate = value;
					this.SendPropertyChanged("CreateDate");
					this.OnCreateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QueueInstanceKey", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid QueueInstanceKey
		{
			get
			{
				return this._QueueInstanceKey;
			}
			set
			{
				if ((this._QueueInstanceKey != value))
				{
					if (this._QueueInstance.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnQueueInstanceKeyChanging(value);
					this.SendPropertyChanging();
					this._QueueInstanceKey = value;
					this.SendPropertyChanged("QueueInstanceKey");
					this.OnQueueInstanceKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ActionTaken", DbType="VarChar(2000)")]
		public string ActionTaken
		{
			get
			{
				return this._ActionTaken;
			}
			set
			{
				if ((this._ActionTaken != value))
				{
					this.OnActionTakenChanging(value);
					this.SendPropertyChanging();
					this._ActionTaken = value;
					this.SendPropertyChanged("ActionTaken");
					this.OnActionTakenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RouteNext", DbType="varchar(50)")]
		public string RouteNext
		{
			get
			{
				return this._RouteNext;
			}
			set
			{
				if ((this._RouteNext != value))
				{
					this.OnRouteNextChanging(value);
					this.SendPropertyChanging();
					this._RouteNext = value;
					this.SendPropertyChanged("RouteNext");
					this.OnRouteNextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="QueueInstance_Request", Storage="_QueueInstance", ThisKey="QueueInstanceKey", OtherKey="QueueInstanceKey", IsForeignKey=true)]
		public QueueInstance QueueInstance
		{
			get
			{
				return this._QueueInstance.Entity;
			}
			set
			{
				if ((this._QueueInstance.Entity != value))
				{
					this.SendPropertyChanging();
					this._QueueInstance.Entity = value;
					this.SendPropertyChanged("QueueInstance");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.QueueInstance")]
	public partial class QueueInstance : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _QueueInstanceID;
		
		private System.Guid _QueueInstanceKey;
		
		private System.Guid _InstanceID;
		
		private System.DateTime _CreateDate;
		
		private System.Nullable<int> _CurrentSubQueueID;
		
		private bool _QC;
		
		private System.Nullable<int> _Priority;
		
		private System.Nullable<System.DateTime> _AssignedDate;
		
		private System.Nullable<int> _AssignedOperatorID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnQueueInstanceIDChanging(int value);
    partial void OnQueueInstanceIDChanged();
    partial void OnQueueInstanceKeyChanging(System.Guid value);
    partial void OnQueueInstanceKeyChanged();
    partial void OnInstanceIDChanging(System.Guid value);
    partial void OnInstanceIDChanged();
    partial void OnCreateDateChanging(System.DateTime value);
    partial void OnCreateDateChanged();
    partial void OnCurrentSubQueueIDChanging(System.Nullable<int> value);
    partial void OnCurrentSubQueueIDChanged();
    partial void OnQCChanging(bool value);
    partial void OnQCChanged();
    partial void OnPriorityChanging(System.Nullable<int> value);
    partial void OnPriorityChanged();
    partial void OnAssignedDateChanging(System.Nullable<System.DateTime> value);
    partial void OnAssignedDateChanged();
    partial void OnAssignedOperatorIDChanging(System.Nullable<int> value);
    partial void OnAssignedOperatorIDChanged();
    #endregion
		
		public QueueInstance()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QueueInstanceID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int QueueInstanceID
		{
			get
			{
				return this._QueueInstanceID;
			}
			set
			{
				if ((this._QueueInstanceID != value))
				{
					this.OnQueueInstanceIDChanging(value);
					this.SendPropertyChanging();
					this._QueueInstanceID = value;
					this.SendPropertyChanged("QueueInstanceID");
					this.OnQueueInstanceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QueueInstanceKey", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid QueueInstanceKey
		{
			get
			{
				return this._QueueInstanceKey;
			}
			set
			{
				if ((this._QueueInstanceKey != value))
				{
					this.OnQueueInstanceKeyChanging(value);
					this.SendPropertyChanging();
					this._QueueInstanceKey = value;
					this.SendPropertyChanged("QueueInstanceKey");
					this.OnQueueInstanceKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InstanceID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid InstanceID
		{
			get
			{
				return this._InstanceID;
			}
			set
			{
				if ((this._InstanceID != value))
				{
					this.OnInstanceIDChanging(value);
					this.SendPropertyChanging();
					this._InstanceID = value;
					this.SendPropertyChanged("InstanceID");
					this.OnInstanceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateDate", DbType="DateTime NOT NULL")]
		public System.DateTime CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				if ((this._CreateDate != value))
				{
					this.OnCreateDateChanging(value);
					this.SendPropertyChanging();
					this._CreateDate = value;
					this.SendPropertyChanged("CreateDate");
					this.OnCreateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CurrentSubQueueID", DbType="Int")]
		public System.Nullable<int> CurrentSubQueueID
		{
			get
			{
				return this._CurrentSubQueueID;
			}
			set
			{
				if ((this._CurrentSubQueueID != value))
				{
					this.OnCurrentSubQueueIDChanging(value);
					this.SendPropertyChanging();
					this._CurrentSubQueueID = value;
					this.SendPropertyChanged("CurrentSubQueueID");
					this.OnCurrentSubQueueIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QC", DbType="Bit NOT NULL")]
		public bool QC
		{
			get
			{
				return this._QC;
			}
			set
			{
				if ((this._QC != value))
				{
					this.OnQCChanging(value);
					this.SendPropertyChanging();
					this._QC = value;
					this.SendPropertyChanged("QC");
					this.OnQCChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Priority", DbType="Int")]
		public System.Nullable<int> Priority
		{
			get
			{
				return this._Priority;
			}
			set
			{
				if ((this._Priority != value))
				{
					this.OnPriorityChanging(value);
					this.SendPropertyChanging();
					this._Priority = value;
					this.SendPropertyChanged("Priority");
					this.OnPriorityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AssignedDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> AssignedDate
		{
			get
			{
				return this._AssignedDate;
			}
			set
			{
				if ((this._AssignedDate != value))
				{
					this.OnAssignedDateChanging(value);
					this.SendPropertyChanging();
					this._AssignedDate = value;
					this.SendPropertyChanged("AssignedDate");
					this.OnAssignedDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AssignedOperatorID", DbType="Int")]
		public System.Nullable<int> AssignedOperatorID
		{
			get
			{
				return this._AssignedOperatorID;
			}
			set
			{
				if ((this._AssignedOperatorID != value))
				{
					this.OnAssignedOperatorIDChanging(value);
					this.SendPropertyChanging();
					this._AssignedOperatorID = value;
					this.SendPropertyChanged("AssignedOperatorID");
					this.OnAssignedOperatorIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
