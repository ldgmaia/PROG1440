/**************************************
* Get the number of times each player played on each position *
**************************************/
select PlayerID, PositionID, COUNT(PositionID) as 'played this position'
from SQUASH.PlayerPositions pp
--where PlayerID = 8 or PlayerID = 16
group by PlayerID, PositionID
order by PlayerID

select pp.PlayerID, pp.PositionID, COUNT(pp.PositionID) as 'played this position', tr.Played
from SQUASH.PlayerPositions pp
join SQUASH.Players p
	on pp.PlayerID = p.ID
join SQUASH.Teams t
	on p.TeamID = t.ID
join SQUASH.TeamRankings tr
	on tr.TeamID = p.TeamID
--where PlayerID = 8 or PlayerID = 16
group by pp.PlayerID, pp.PositionID, tr.Played
order by pp.PlayerID

/**************************************
* Get the number of fixtures played by team
**************************************/
declare @teamID as int
set @teamID = 1
select(
(select count(f.HomeTeamID) homeFixture
from SQUASH.Fixtures f
where f.HomeTeamID = @teamID
group by f.HomeTeamID)
+
(select count(f.AwayTeamBonus) homeFixture
from SQUASH.Fixtures f
where f.AwayTeamID = @teamID
group by f.AwayTeamID)
) as total

-- OR
declare @teamID as int
set @teamID = 1

declare @homeFixtures as int
declare @awayFixtures as int

set @homeFixtures = (select count(f.HomeTeamID) homeFixture
from SQUASH.Fixtures f
where f.HomeTeamID = @teamID
group by f.HomeTeamID)

set @awayFixtures = (select count(f.AwayTeamBonus) homeFixture
from SQUASH.Fixtures f
where f.AwayTeamID = @teamID
group by f.AwayTeamID)

select (@homeFixtures + @awayFixtures) as total

select *
from SQUASH.Fixtures