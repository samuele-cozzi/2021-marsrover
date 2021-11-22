/*
truncate table [dbo].Commands
truncate table [dbo].Positions
truncate table dbo.Landing
*/



select * from [dbo].Commands order by timestamp
select * from [dbo].Positions order by timestamp
select * from dbo.Landing


select * from EventEntity
select * from SnapshotEntity
