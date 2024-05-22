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

            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();
            return RedirectToPage("/DisplayRooms");
        }
    }

    
}