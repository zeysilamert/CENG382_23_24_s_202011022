using System.Text.Json;
using System.Text.Json.Serialization;

/*In Lab 6, my code violated the Single Responsibility Principle and Dependency Injection Principle 
because some classes were handling multiple responsibilities, leading to a complex dependency structure 
where classes relied on too many other classes to perform their functions.*/

// States for you to try
public class States
{
    public string name { get; set; }
    public string roomNumber { get; set; }
    public string enterDay { get; set; }
    public string enterHour { get; set; }

    public States(string Name, string RoomNumber, string Enter, string Exit)
    {
        name = Name;
        roomNumber = RoomNumber;
        enterDay = Enter;
        enterHour = Exit;
    }
    public void displayProperty()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Room Number: {roomNumber}");
        Console.WriteLine($"Entrance Day: {enterDay}");
        Console.WriteLine($"Entrance Time: {enterHour}\n");
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";
        string logFilePath = "LogData.Json";

        ILogger logFile = new FileLogger(logFilePath);
        LogHandler logHandler= new LogHandler(logFile);
        IReservationRepository reservationRepository = new ReservationRepository();
        RoomHandler roomHandler = new RoomHandler(jsonFilePath);

        try
        {

            Dictionary<string, States> statesDictionary = new Dictionary<string, States>
            {
                {"Zeynep", new States("Zeynep", "002", "Monday", "11:20")},
                {"Sila", new States("Sila", "001", "Monday", "11:20")},
                {"Mert", new States("Mert", "001", "Monday", "11:00")},
                {"Isinsu", new States("Isinsu", "003", "Tuesday", "10:00")},
                {"Tuna", new States("Tuna", "003", "Friday", "11:00")}
            };

            var roomData = roomHandler.GetRooms();
            ReservationHandler handler = new ReservationHandler(roomData, reservationRepository, logHandler);
            ReservationService reservationService= new ReservationService(handler, reservationRepository);

            bool programOn = true;
            while (programOn)
            {
                Console.WriteLine("To add a new reservation press 1.");
                Console.WriteLine("To delete reservation press 2.");
                Console.WriteLine("To display weekly schedule press 3.");
                Console.WriteLine("To exit press 4.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:
                        int index = 1;
                        foreach (var state in statesDictionary)
                        {
                            Console.WriteLine($"Select state {index}:");
                            state.Value.displayProperty();
                            index++;
                        }

                        Console.WriteLine("Enter the number of the state to select:");
                        if (int.TryParse(Console.ReadLine(), out int stateSelection) && stateSelection > 0 && stateSelection <= statesDictionary.Count)
                        {
                            States selectedState = statesDictionary.ElementAt(stateSelection - 1).Value;

                            string reserverName = selectedState.name;
                            string roomNumber = selectedState.roomNumber;
                            string day = selectedState.enterDay;
                            DateTime time = DateTime.Parse(selectedState.enterHour);

                            handler.AddReservation(day, roomNumber, reserverName, time);
                        }

                        break;

                    case 2:
                        Console.Write("\nEnter guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();

                        if (statesDictionary.TryGetValue(reserverNameToDelete, out States providedState))
                        {   
                            string reserverName = providedState.name;
                            string roomNumber = providedState.roomNumber;
                            string day = providedState.enterDay;
                            DateTime time = DateTime.Parse(providedState.enterHour);
                            handler.DeleteReservationByName(reserverNameToDelete, roomNumber, day, time);
                        }

                        break;

                    case 3:
                        handler.PrintWeeklySchedule();
                        break;

                    case 4:
                        programOn = false;
                        Console.WriteLine("\nThank you for using us!");
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again.");
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