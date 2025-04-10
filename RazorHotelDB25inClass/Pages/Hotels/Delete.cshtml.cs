using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Hotels
{
    public class DeleteModel : PageModel
    {
        private IHotelServiceAsync _hotelService;
        [BindProperty]
        public Hotel Hotel { get; set; }
        [BindProperty] public bool Confirm { get; set; }
        public string MessageError { get; set; }
        public DeleteModel(IHotelServiceAsync hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task<IActionResult> OnGetAsync(int deleteId)
        {
            Hotel = await _hotelService.GetHotelFromIdAsync(deleteId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Confirm == false)
            {
                MessageError = $"Remember to check the Confirm box";
                return Page();
            }
            await _hotelService.DeleteHotelAsync(Hotel.HotelNr);
            return RedirectToPage("GetAllHotels");
        }
    }
}
