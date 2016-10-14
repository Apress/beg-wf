IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'QueueTrack')
	BEGIN
		DROP  Table QueueTrack
	END
GO

CREATE TABLE QueueTrack
(
   QueueTrackID int identity not null primary key,
   EventType varchar(20) not null,
   QueueInstanceKey uniqueidentifier not null,
   SubQueueID int null,
   QC bit null,
   OperatorKey uniqueidentifier null,
   EventDate datetime not null,
)
GO

-- Add the indexes
CREATE INDEX IX_QueueTrack_1 ON QueueTrack(QueueInstanceID)
GO
CREATE INDEX IX_QueueTrack_2 ON QueueTrack(SubQueueID)
GO
CREATE INDEX IX_QueueTrack_3 ON QueueTrack(OperatorKey)
GO

-- Add the foreign keys
ALTER TABLE QueueTrack 
    ADD CONSTRAINT FK_QueueTrack_SubQueue
    FOREIGN KEY (SubQueueID) 
    REFERENCES SubQueue (SubQueueID)
GO


