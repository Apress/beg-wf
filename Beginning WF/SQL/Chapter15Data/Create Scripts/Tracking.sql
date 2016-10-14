IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'TrackInstance')
	BEGIN
		DROP  Table TrackInstance
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'TrackActivity')
	BEGIN
		DROP  Table TrackActivity
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'TrackBookmark')
	BEGIN
		DROP  Table TrackBookmark
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'TrackCustom')
	BEGIN
		DROP  Table TrackCustom
	END
GO

CREATE TABLE TrackInstance
(
    TrackInstanceID int identity not null primary key,
    WorkflowID uniqueidentifier not null,
	EventDate datetime not null,
    Status varchar(20) not null
)
GO

CREATE INDEX IX_TrackInstance_1 ON TrackInstance(WorkflowID)
GO

CREATE TABLE TrackActivity
(
    TrackActivityID int identity not null primary key,
    WorkflowID uniqueidentifier not null,
    ActivityName varchar(80) not null,
	EventDate datetime not null,
    Status varchar(20) not null,
    Variables varchar(2000) null
)
GO

CREATE INDEX IX_TrackActivity_1 ON TrackActivity(WorkflowID)
GO
CREATE INDEX IX_TrackActivity_2 ON TrackActivity(ActivityName)
GO


CREATE TABLE TrackBookmark
(
    TrackBookmarkID int identity not null primary key,
    WorkflowID uniqueidentifier not null,
    BookmarkName varchar(80) null,
	EventDate datetime not null
)
GO

CREATE INDEX IX_TrackBookmark_1 ON TrackBookmark(WorkflowID)
GO
CREATE INDEX IX_TrackBookmark_2 ON TrackBookmark(BookmarkName)
GO

CREATE TABLE TrackCustom
(
    TrackCustomID int identity not null primary key,
    WorkflowID uniqueidentifier not null,
    CustomEventName varchar(80) not null,
	EventDate datetime not null,
	UserData varchar(2000) null
)
GO

CREATE INDEX IX_TrackCustom_1 ON TrackCustom(WorkflowID)
GO
CREATE INDEX IX_TrackCustom_2 ON TrackCustom(CustomEventName)
GO