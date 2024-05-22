using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;

namespace MyApp.Namespace
{
    public class DisplayRoomsModel : PageModel
    {

        private readonly WebAppDataBaseContext _context;

        public DisplayRoomsModel(WebAppDataBaseContext context)
        {
            _context = context;
        }
        public List <Room> NewRoomList { get; set; } = new List<Room>();
        public void OnGet()
        {
            NewRoomList = _context.Rooms.ToList();
        }
    }
}