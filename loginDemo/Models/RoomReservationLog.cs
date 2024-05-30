using loginDemo.Models;

public class RoomReservationLog
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int? ReservationId { get; set; }
    public int? RoomId {get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime? ReservationDate { get; set; }
    public virtual Reservation? Reservation { get; set; }
    public virtual Room? Room { get; set; }
}