using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Rooms
{
    public class CreateModel : PageModel
    {
        private IRoomServiceAsync _roomService;

        [BindProperty]
        public Room Room { get; set; }
        [BindProperty] public int HotelNr { get; set; }
        public string ErrorMessage { get; set; }

        public CreateModel(IRoomServiceAsync roomService)
        {
            _roomService = roomService;
        }

        public void OnGet(int id)
        {
            HotelNr = id;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            try //try-catch for at fange exceptions.
            {
                await _roomService.CreateRoomAsync(HotelNr, Room);
                return RedirectToPage("GetAllRooms", new { HotelNr = HotelNr });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
    }
}
