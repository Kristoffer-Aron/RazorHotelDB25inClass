using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private IRoomServiceAsync _roomService;
        [BindProperty] public Room Room { get; set; }
        [BindProperty] public bool Confirm { get; set; }
        [BindProperty] public int HotelNr { get; set; }
        public string MessageError { get; set; }
        public DeleteModel(IRoomServiceAsync roomService)
        {
            _roomService = roomService;
        }
        public async Task<IActionResult> OnGetAsync(int deleteId, int roomId)
        {
            HotelNr = deleteId;
            Room = await _roomService.GetRoomFromIdAsync(roomId, deleteId);
            //Room.HotelNr = deleteId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Confirm == false)
            {
                MessageError = $"Remember to check the Confirm box";
                return Page();
            }
            /*HotelNr = Room.HotelNr;*/ //Weird error where HotelNr is set to 0 at start of OnPost(). Workaround.
            await _roomService.DeleteRoomAsync(Room.RoomNr, Room.HotelNr);
            return RedirectToPage("GetAllRooms", new { HotelNr = Room.HotelNr });
        }
    }
}
