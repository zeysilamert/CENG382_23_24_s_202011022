public record LogRecord{
    DateTime endTime { get; init; }
    string dayName { get; init; }
    string ReserverName { get; init; }
    string RoomName { get; init; }
    string action { get; init; }// "Added" or "Deleted"

    public LogRecord(DateTime endTime, string dayName, string ReserverName, string RoomName, string action){
        this.endTime = endTime;
        this.dayName = dayName;
        this.ReserverName = ReserverName;
        this.RoomName = RoomName;
        this.action = action;
    }
}
    

