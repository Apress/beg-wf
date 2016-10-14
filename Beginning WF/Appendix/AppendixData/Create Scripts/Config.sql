IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Request')
	BEGIN
		DROP  Table Request
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'QueueInstance')
	BEGIN
		DROP  Table QueueInstance
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'SubQueue')
	BEGIN
		DROP  Table SubQueue
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Queue')
	BEGIN
		DROP  Table Queue
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'OperatorConfig')
	BEGIN
		DROP  Table OperatorConfig
	END
GO

-------------------------------------------------
--
-- Queue
--
-------------------------------------------------
CREATE TABLE Queue
(
   QueueID int identity not null primary key,
   QueueName varchar(20) not null,
   SupportsQC bit default 1 not null
)
GO
CREATE UNIQUE INDEX IX_Queue ON Queue(QueueName)
GO

-------------------------------------------------
--
-- SubQueue
--
-------------------------------------------------
CREATE TABLE SubQueue
(
   SubQueueID int identity not null primary key,
   QueueID int not null,
   SubQueueName varchar(20) not null,
   AllowSelection bit default 0 not null,
   Frequency int not null,
   NumberSinceLastEval int default 0 not null
)
GO
CREATE INDEX IX_SubQueue_1 ON SubQueue(QueueID)
GO
CREATE INDEX IX_SubQueue_2 ON SubQueue(SubQueueName)
GO
ALTER TABLE SubQueue
    ADD CONSTRAINT CS_SubQueue UNIQUE (QueueID, SubQueueName)
GO
ALTER TABLE SubQueue 
    ADD CONSTRAINT FK_SubQueue_Queue
    FOREIGN KEY (QueueID) 
    REFERENCES Queue (QueueID)
GO

-------------------------------------------------
--
-- OperatorConfig
--
-------------------------------------------------
CREATE TABLE OperatorConfig
(
   OperatorConfigID int identity not null primary key,
   OperatorKey uniqueidentifier not null,
   UnderEvaluation bit not null,
   Frequency int not null,
   NumberSinceLastEval int default 0 not null
)
GO
CREATE UNIQUE INDEX IX_OperatorConfig_1 ON OperatorConfig(OperatorKey)
GO

-------------------------------------------------
--
-- QueueInstance
--
-------------------------------------------------
CREATE TABLE QueueInstance
(
   QueueInstanceID int identity not null primary key,
   QueueInstanceKey uniqueidentifier not null,
   InstanceID uniqueidentifier not null,
   CreateDate datetime not null,
   CurrentSubQueueID int null,
   QC bit default 0 not null,
   Priority int null,
   AssignedDate datetime null,
   AssignedOperatorID int null
)
GO
CREATE UNIQUE INDEX IX_QueueInstance_1 ON QueueInstance(QueueInstanceKey)
GO
CREATE INDEX IX_QueueInstance_2 ON QueueInstance(InstanceID)
GO
CREATE INDEX IX_QueueInstance_3 ON QueueInstance(CurrentSubQueueID)
GO
CREATE INDEX IX_QueueInstance_4 ON QueueInstance(CurrentSubQueueID, QC)
GO
CREATE INDEX IX_QueueInstance_5 ON QueueInstance(AssignedOperatorID)
GO
-- Setup the foreign key relationships
ALTER TABLE QueueInstance 
    ADD CONSTRAINT FK_QueueInstance_SubQueue
    FOREIGN KEY (CurrentSubQueueID) 
    REFERENCES SubQueue (SubQueueID)
GO
ALTER TABLE QueueInstance 
    ADD CONSTRAINT FK_QueueInstance_Operator
    FOREIGN KEY (AssignedOperatorID) 
    REFERENCES OperatorConfig (OperatorConfigID)
GO

-------------------------------------------------
--
-- Setup the configuration data
--
-------------------------------------------------
-- Populate the Queue table
insert into Queue (QueueName, SupportsQC) values ('Request', 1)

-- Populate the SubQueue table
insert into SubQueue (QueueID, SubQueueName, AllowSelection, Frequency) 
   select QueueID, 'Product', 0, 1   from Queue where QueueName = 'Request'
insert into SubQueue (QueueID, SubQueueName, AllowSelection, Frequency) 
   select QueueID, 'Service', 0, 4   from Queue where QueueName = 'Request'
insert into SubQueue (QueueID, SubQueueName, AllowSelection, Frequency) 
   select QueueID, 'Marketing', 1, 5 from Queue where QueueName = 'Request'
insert into SubQueue (QueueID, SubQueueName, AllowSelection, Frequency) 
   select QueueID, 'General', 0, 8   from Queue where QueueName = 'Request'


