--Permissions needed to use sql notifications
grant control on schema::dbo to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant create procedure to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant create queue to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant create service to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant impersonate on user::dbo to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant select on [System.Activities.DurableInstancing].[CommandQueueTable] to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant references on contract::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go

grant subscribe query notifications to [System.Activities.DurableInstancing.WorkflowManagementSystem] 
go

grant receive on QueryNotificationErrorsQueue to [System.Activities.DurableInstancing.WorkflowManagementSystem]
go