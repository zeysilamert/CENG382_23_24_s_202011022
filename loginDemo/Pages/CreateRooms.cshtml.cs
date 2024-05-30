using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using loginDemo.Models;

namespace MyApp.Namespace
{
    [Authorize]
    public class CreateRoomsModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public CreateRoomsModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room NewRoom { get; set; } = new Room(); // Initialize the Room property

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewRoom == null)
            {
                return Page();
            }

            // Check for duplicate room name
            if (_context.Rooms.Any(r => r.RoomName == NewRoom.RoomName))
            {
                ModelState.AddModelError("NewRoom.RoomName", "A room with this name already exists.");
                return Page();
            }

            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();

            var log = new RoomReservationLog
            {
                Timestamp = DateTime.Now,
                RoomId = NewRoom.Id,
                UserId = User.Identity.Name
            };

            _context.RoomReservationLogs.Add(log);
            _context.SaveChanges();

            return RedirectToPage("/DisplayRooms");
        }
    }

    
}