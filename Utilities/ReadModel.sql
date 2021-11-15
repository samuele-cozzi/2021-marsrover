/*
truncate table [dbo].Commands
truncate table [dbo].Positions

*/



select * from [dbo].Commands order by timestamp
select * from [dbo].Positions order by timestamp
select * from [dbo].Commands s inner join [dbo].Positions p on s.AggregateId = p.StartId order by s.timestamp
select * from [dbo].Commands where Move <> 'f-f-f-f' order by timestamp
select * from [dbo].Positions where IsBlocked = 1 order by timestamp

select * from EventEntity
select * from SnapshotEntity
