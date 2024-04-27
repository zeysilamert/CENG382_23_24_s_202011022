public class ReservationHandler
{
    private IReservationRepository reservationRepository;
    private LogHandler logHandler;
    private Dictionary<string, Dictionary<Room, List<(DateTime, string)>>> weeklyReservations;
    private TimeSpan breakTime = TimeSpan.FromMinutes(40);

    public ReservationHandler(RoomData roomData, ReservationRepository reservationRepository, LogHandler logHandler)
    {
        weeklyReservations = new Dictionary<string, Dictionary<Room, List<(DateTime, string)>>>();

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            weeklyReservations[day.ToString()] = new Dictionary<Room, List<(DateTime, string)>>();
        }

        foreach (Room room in roomData.Rooms)
        {
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek day = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
                weeklyReservations[day.ToString()][room] = new List<(DateTime, string)>();
            }
        }
        this.reservationRepository = reservationRepository;
        this.logHandler = logHandler;
    }

    public void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime)
    {
        Room room = Array.Find(weeklyReservations[day].Keys.ToArray(), r => r.RoomId == roomNumber);
        List<(DateTime, string)> reservations = weeklyReservations[day][room];
        Reservation reservation;

        DateTime endTime = enterTime.AddMinutes(40); 

        if (reservations.Any(reservation => enterTime < reservation.Item1.Add(breakTime) && endTime > reservation.Item1))
        {
            Console.WriteLine("There is a reservation conflict. Please choose another time.");
            return;
        }

        reservations.Add((enterTime, reserverName));
        //reservation = new Reservation(day, endTime, room, reserverName);
        Console.WriteLine($"Reservation added for room {roomNumber} on {day} at {enterTime:hh:mm tt}.");
        //reservationRepository.AddReservation(reservation);
        var log = new LogRecord(endTime, day, reserverName, roomNumber, $"Added Reservation: {roomNumber}");
        logHandler.AddLog(log);
        
    }


    public void DeleteReservationByName(string reserverName)
    {
        foreach (var dayReservations in weeklyReservations.Values)
        {
            foreach (var roomReservations in dayReservations.Values)
            {
                roomReservations.RemoveAll(reservation => reservation.Item2 == reserverName);
            }
        }
        Console.WriteLine($"All reservations for guest {reserverName} deleted.");
    }

    public void PrintWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule:");

        
        for (int i = 0; i < 7; i++)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
            string dayOfWeekString = dayOfWeek.ToString();

            Console.WriteLine($"Day: {dayOfWeekString}");

            foreach (var roomKvp in weeklyReservations[dayOfWeekString])
            {
                Room room = roomKvp.Key;
                List<(DateTime, string)> reservations = roomKvp.Value;

                if (reservations.Count == 0)
                {
                    continue;
                }

                Console.WriteLine($"Room {room.RoomId} ({room.RoomName}):");

                foreach ((DateTime time, string reserverName) in reservations)
                {
                    Console.WriteLine($"  {time:hh:mm tt} - {reserverName}");
                }
            }

            Console.WriteLine();
        }
    }
}