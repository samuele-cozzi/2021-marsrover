/*
truncate table [dbo].Commands
truncate table [dbo].Positions
truncate table dbo.Landing
*/


-- drop database RoverRM
-- create database RoverRM
-- drop database Jobs
-- create database Jobs

select * from [RoverRM].[dbo].Commands order by timestamp
select * from [RoverRM].[dbo].Positions order by timestamp
select * from [RoverRM].dbo.Landing

select * FROM [RoverRM].[dbo].[EventFlow]

-- select * from EventEntity
-- select * from SnapshotEntity
