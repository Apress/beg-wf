IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Assignment')
	BEGIN
		DROP  Table Assignment
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Lead')
	BEGIN
		DROP  Table Lead
	END
GO
-------------------------------------------------
-- Create the Lead table
-------------------------------------------------
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

-------------------------------------------------
-- Create the Assignment table
-------------------------------------------------
CREATE TABLE Assignment
(
	AssignmentID int identity not null primary key,
	WorkflowID uniqueidentifier not null,
	LeadID int not null,
	AssignedTo varchar(50) not null,
	DateAssigned datetime not null,
	Status varchar(20) not null,
	DateDue datetime null,
	DateCompleted datetime null,
	Remarks varchar(200) null
)
GO

CREATE INDEX IX_Assignment ON Assignment(LeadID)
GO

-- Setup the foreign key relationships
ALTER TABLE Assignment 
    ADD CONSTRAINT FK_Assignment_Lead
    FOREIGN KEY (LeadID) 
    REFERENCES Lead (LeadID)
GO
