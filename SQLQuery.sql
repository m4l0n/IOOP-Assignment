/*Declare @date Date, @days INT = 6, @counter INT = 1
Declare @Results TABLE (Date Date, TotalReservation int)
WHILE (@counter < @days)
BEGIN
	Declare @currentDate Date = DateAdd(Day, -@counter, getdate())
	insert into @Results (Date, TotalReservation)
	select Date, count(*) as TotalReservation
	from [dbo].Reservation
	where Date=@currentDate
	group by Date
	Set @counter += 1
END
*/

/*select Date, count(*) as TotalReservation
from [dbo].Reservation
where Date between DateAdd(Day, -5, getdate()) and getDate()
group by Date*/

create procedure GetPastReservation
as
    WITH dateTable AS (
        SELECT 
             CAST(GETDATE() AS DATE )Date
        UNION ALL   
        SELECT 
             DATEADD(DAY,-1,dateTable.Date) Date
        FROM dateTable
        WHERE Date >= GETDATE() - 4
    )
    SELECT 
         dateTable.Date
        ,COUNT(Reservation.Date) as TotalReservation
    FROM dateTable
    LEFT OUTER JOIN Reservation ON dateTable.Date = Reservation.Date
    GROUP by dateTable.Date
    ORDER BY dateTable.Date;
