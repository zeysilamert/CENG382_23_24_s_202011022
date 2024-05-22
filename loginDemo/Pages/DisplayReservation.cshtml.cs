using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using loginDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Namespace
{
    [Authorize]
    public class DisplayReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public DisplayReservationModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservations { get; set; }

        public void OnGet(string roomName, DateTime? startDate, int? capacity)
        {
            var query = _context.Reservations.Include(r => r.Room).AsQueryable();  // Include Room data

            if (!string.IsNullOrEmpty(roomName))
            {
                query = query.Where(r => r.Room.RoomName == roomName);
            }

            if (startDate.HasValue)
            {
                var endDate = startDate.Value.AddDays(7);
                query = query.Where(r => r.DateTime >= startDate.Value && r.DateTime <= endDate);
            }

            if (capacity.HasValue)
            {
                query = query.Where(r => r.Room.Capacity >= capacity);
            }

            Reservations = query.ToList();
        }
    }
}