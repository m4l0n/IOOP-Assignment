Declare @date Date, @days INT = 6, @counter INT = 1
Declare @Results TABLE (Date Date, TotalReservation int)
WHILE (@counter < @days)
BEGIN
	Declare @currentDate Date = DateAdd(Day, -@counter, getdate())
	insert into @Results (Date, TotalReservation)
	select Date, COALESCE(count(ReservationID),0) as TotalReservation
	from [dbo].Reservation
	where Date=@currentDate
	group by Date
	Set @counter += 1
END
select * from @Results
