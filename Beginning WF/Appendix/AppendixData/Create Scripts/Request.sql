IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Request')
	BEGIN
		DROP  Table Request
	END
GO

CREATE TABLE Request
(
   RequestID int identity not null primary key,
   RequestKey uniqueidentifier not null,
   RequestType varchar(20) not null,
   UserName varchar(100),
   UserEmail varchar(70),
   Comment varchar(2000),
   CreateDate datetime not null,
   QueueInstanceKey uniqueidentifier not null,
   ActionTaken varchar(2000) null,
   RouteNext varchar(50) null
)
GO

-- Add the indexes
CREATE INDEX IX_Request_1 ON Request(RequestKey)
GO
CREATE INDEX IX_Request_2 ON Request(QueueInstanceKey)
GO

-- Setup the foreign key relationships
--ALTER TABLE Request 
--    ADD CONSTRAINT FK_Request_QueueInstance
--    FOREIGN KEY (QueueInstanceKey) 
--    REFERENCES QueueInstance (QueueInstanceKey)
--GO



