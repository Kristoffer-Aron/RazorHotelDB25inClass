using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB25inClass.Exceptions;
using RazorHotelDB25inClass.Interfaces;
using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Pages.Hotels
{
    public class CreateModel : PageModel
    {
        private IHotelServiceAsync _hotelService;

        [BindProperty]
        public Hotel Hotel { get; set; }
        public string ErrorMessage { get; set; }

        public CreateModel(IHotelServiceAsync hotelService)
        {
            _hotelService = hotelService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) { return Page(); } 
            try //try-catch for at fange exceptions.
            {
                await _hotelService.CreateHotelAsync(Hotel);
                return RedirectToPage("GetAllHotels");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
    }
}
