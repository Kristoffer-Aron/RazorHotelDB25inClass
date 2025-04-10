using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25InClass.Helpers;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;
using RazorHotelDB25inClass.Helper;

namespace RazorHotelDB25inClass.Pages.Rooms
{
    public class GetAllRoomsModel : PageModel
    {
        private IRoomServiceAsync _roomService;
        private IHotelServiceAsync _hotelService;
        public List<Room> Rooms { get; set; }
        [BindProperty] public int HotelNr { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; }

        public GetAllRoomsModel(IRoomServiceAsync roomService, IHotelServiceAsync hotelService)
        {
            SortOrder = "RoomNr";
            Rooms = new List<Room>();
            _roomService = roomService;
            _hotelService = hotelService;
        }

        public async Task OnGetAsync(int hotelNr) // OnGet kører når siden indlæses
        {
            HotelNr = hotelNr;
            try
            {
                Rooms = await _roomService.GetAllRoomAsync(HotelNr);   
                if (SortBy == "RoomNr") { Rooms.Sort(); }
                if (SortBy == "Types") { Rooms.Sort(new RoomTypesCompare()); }
                if (SortOrder == "Descending") { Rooms.Reverse(); }
            }
            catch (Exception ex)
            {
                Rooms = new List<Room>();
                ViewData["ErrorMessage"] = ex.Message;
            }

        }
        
    }
}
