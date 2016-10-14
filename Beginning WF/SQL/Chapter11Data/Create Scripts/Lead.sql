IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Lead')
BEGIN
    DROP  Table Lead
END
GO

CREATE TABLE Lead
(
    LeadID int identity not null primary key,
    WorkflowID uniqueidentifier not null,
    ContactName varchar(80) not null,
    ContactPhone varchar(20) null,
    Interests varchar(100) null,
    Comments varchar(500) null,
    Status varchar(20) not null,
    AssignedTo varchar(50) null
)
GO

CREATE  UNIQUE INDEX IX_Lead ON Lead(WorkflowID)
GO
