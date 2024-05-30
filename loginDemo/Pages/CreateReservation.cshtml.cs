using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;
using NuGet.Packaging.Signing;
using Microsoft.Extensions.Configuration.UserSecrets;


namespace MyApp.Namespace
{
    [Authorize]
    public class CreateReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;
        //private readonly ILogger<CreateReservationModel> _logger;

        public CreateReservationModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation();

        public List<Room> Rooms { get; set; }

        public void OnGet()
        {
            Rooms = _context.Rooms.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Rooms = _context.Rooms.ToList();
                return Page();
            }

            if (Reservation.DateTime.Minute != 0 || Reservation.DateTime.Second != 0)
            {
                ModelState.AddModelError("Reservation.DateTime", "Please select a time at the top of the hour.");
                Rooms = _context.Rooms.ToList();
                return Page();
            }

            bool isConflict = _context.Reservations
                .Any(r => r.DateTime == Reservation.DateTime &&
                          !r.IsDeleted);

            if (isConflict)
            {
                ModelState.AddModelError("Reservation.DateTime", "A reservation for this room at the same date and time already exists.");
                Rooms = _context.Rooms.ToList(); 
                return Page();
            }

            Reservation.ReservedBy = User.Identity.Name; 
            _context.Reservations.Add(Reservation);
            _context.SaveChanges();

            
            //_logger.LogInformation("Reservation created by {UserName} for Room ID {RoomId} on {DateTime}", User.Identity.Name, Reservation.RoomId, Reservation.DateTime);

            var log = new RoomReservationLog
            {
                Timestamp = DateTime.Now,
                ReservationId = Reservation.Id,
                RoomId = Reservation.RoomId,
                UserId = Reservation.ReservedBy,
                ReservationDate = Reservation.DateTime
            };

            _context.RoomReservationLogs.Add(log);
            _context.SaveChanges();

            return RedirectToPage("/DisplayReservation");
        }
    }
}