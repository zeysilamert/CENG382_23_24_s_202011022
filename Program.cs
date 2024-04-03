using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; }
}

public class Room
{
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    [JsonPropertyName("roomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}

public class Reservation
{
    public Room Room { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string ReserverName { get; set; }
}

public class ReservationHandler
{
    private Reservation[,] reservations;
    private RoomData roomData;

    public ReservationHandler(int days, int numberOfRooms, RoomData roomData_sent)
    {
        reservations = new Reservation[numberOfRooms, days];
        roomData = roomData_sent;
    }

    public void AddReservation(Reservation reservation)
    {

        int startIndex = reservation.Date.Day - 1;
        int numberOfDays = reservation.Time.Day - startIndex;
        int roomIndex = int.Parse(reservation.Room.RoomId) - 1;

        if (startIndex < 0 || startIndex + numberOfDays > reservations.GetLength(1))
        {
            Console.WriteLine("This booking falls outside the month's range.");
            return;
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            if (reservations[roomIndex, i] != null)
            {
                Console.WriteLine($"Reservation conflict on {reservation.Date.AddDays(i - startIndex):MM/dd/yyyy}");
                return;
            }
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            reservations[roomIndex, i] = reservation;
        }

        Console.WriteLine($"Reserved dates: from {reservation.Date:MM/dd/yyyy} to {reservation.Time:MM/dd/yyyy} for room {reservation.Room.RoomName}.");
    }

    public void DeleteReservation(string reserverName)
    {
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j]?.ReserverName == reserverName)
                {
                    reservations[i, j] = null;
                }
            }
        }

        Console.WriteLine($"All reservations for {reserverName} are deleted.");
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule:");
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            Console.WriteLine($"Room {i + 1} - {roomData.Rooms[i].RoomName}:");
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j] != null && reservations[i, j] != reservations[i, j + 1])
                {
                    Console.WriteLine($"  {reservations[i, j].Date:MM/dd/yyyy} to {reservations[i, j].Time:MM/dd/yyyy}: {reservations[i, j].ReserverName}");
                }
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";

        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            ReservationHandler handler = new ReservationHandler(28, roomData.Rooms.Length, roomData);

            bool programOn = true;
            while (programOn)
            {
                Console.WriteLine("Press 1 to add a new reservation.");
                Console.WriteLine("Press 2 to delete reservation.");
                Console.WriteLine("Press 3 to display weekly schedule.");
                Console.WriteLine("Press 4 to exit.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:

                        Console.WriteLine("All the Rooms:");
                        for (int i = 0; i < roomData.Rooms.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {roomData.Rooms[i].RoomName}");
                        }

                        Console.Write("Enter a reserver name: ");
                        string reserverName = Console.ReadLine();

                        Console.Write("Enter room number: ");
                        int roomIndex = int.Parse(Console.ReadLine()) - 1;

                        if (roomIndex < 0 || roomIndex >= roomData.Rooms.Length)
                        {
                            Console.WriteLine("Invalid room selection, please try again.");
                            break;
                        }

                        Console.Write("Enter entrance day (MM/DD/YYYY): ");
                        DateTime date = DateTime.Parse(Console.ReadLine());

                        Console.Write("Enter exit day (MM/DD/YYYY): ");
                        DateTime time = DateTime.Parse(Console.ReadLine());

                        Reservation newReservation = new Reservation
                        {
                            Date = date,
                            Time = time,
                            ReserverName = reserverName,
                            Room = roomData.Rooms[roomIndex]
                        };

                        handler.AddReservation(newReservation);

                        break;

                    case 2:
                        Console.Write("To delete a reservation, please provide the name of the reserver: ");
                        string reserverNameToDelete = Console.ReadLine();
                        handler.DeleteReservation(reserverNameToDelete);
                        
                        break;

                    case 3:
                        handler.DisplayWeeklySchedule();
                        break;

                    case 4:
                        programOn = false;
                        Console.WriteLine("Thank you for using.");
                        break;
                    default:
                        Console.WriteLine("Invalid value, please try again.");
                        break;
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File '{jsonFilePath}' not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Error deserializing JSON data from file '{jsonFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
