using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Hotels
{
    public class EditModel : PageModel
    {
        private IHotelServiceAsync _hotelService;
        [BindProperty]
        public Hotel Hotel { get; set; }
        public EditModel(IHotelServiceAsync hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Hotel = await _hotelService.GetHotelFromIdAsync(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _hotelService.UpdateHotelAsync(Hotel, Hotel.HotelNr);
            return RedirectToPage("GetAllHotels");
        }
    }
}
