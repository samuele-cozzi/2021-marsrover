/*
truncate table [dbo].[ReadModel-Start]
truncate table [dbo].[ReadModel-Position]

drop table [dbo].[ReadModel-Start]
drop table [dbo].[ReadModel-Position]
*/

CREATE TABLE [dbo].[ReadModel-Start] (
	[AggregateId] [nvarchar](50) NOT NULL,
	[Timestamp] DATETIME NOT  NULL,	
	[SequenceNumber] INT NOT NULL,
	[Move] VARCHAR(MAX)  NULL,
 CONSTRAINT [PK_ReadModel-Start_1] PRIMARY KEY CLUSTERED 
(
	[AggregateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];



CREATE TABLE [dbo].[ReadModel-Position] (
	[AggregateId] [nvarchar](50) NOT NULL,
	[Timestamp] DATETIME NOT  NULL,	
	[SequenceNumber] INT NOT NULL,
	[Latitude] NUMERIC(15,6) NULL,
	[Longitude] NUMERIC(15,6)  NULL,
	[FacingDirection] [nvarchar](1) NULL,
    [StartId] [nvarchar](50) NULL,
	[IsBlocked] BIT NULL,
 CONSTRAINT [PK_ReadModel-Position_1] PRIMARY KEY CLUSTERED 
(
	[AggregateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];


select * from [dbo].[ReadModel-Start] order by timestamp
select * from [dbo].[ReadModel-Position] order by timestamp
select * from [dbo].[ReadModel-Start] s inner join [dbo].[ReadModel-Position] p on s.AggregateId = p.StartId order by s.timestamp
select * from [dbo].[ReadModel-Start] where Move <> 'f-f-f-f' order by timestamp
select * from [dbo].[ReadModel-Position] where IsBlocked = 1 order by timestamp

