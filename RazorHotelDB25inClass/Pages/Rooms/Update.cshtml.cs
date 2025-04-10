using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Rooms
{
    public class UpdateModel : PageModel
    {
        private IRoomServiceAsync _roomService;
        [BindProperty]
        public Room Room { get; set; }
        public UpdateModel(IRoomServiceAsync roomService)
        {
            _roomService = roomService;
        }
        public async Task<IActionResult> OnGetAsync(int roomNr, int hotelNr)
        {
            Room = await _roomService.GetRoomFromIdAsync(roomNr, hotelNr);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _roomService.UpdateRoomAsync(Room, Room.RoomNr, Room.HotelNr);
            return RedirectToPage("GetAllRooms", new { HotelNr = Room.HotelNr });
        }
    }
}
