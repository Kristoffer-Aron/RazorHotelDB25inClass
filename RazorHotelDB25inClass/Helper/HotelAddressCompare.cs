using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25InClass.Helpers
{
    public class HotelAddressCompare : IComparer<Hotel>
    {
        public int Compare(Hotel? x, Hotel? y)
        {
            if (x == null && y == null) { return 0; }
            else if (x == null) { return -1; }
            else if (y == null) { return 1; }
            return string.Compare(x.Adresse, y.Adresse);
        }
    }
}