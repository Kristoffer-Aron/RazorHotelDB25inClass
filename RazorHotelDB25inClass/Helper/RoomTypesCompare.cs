using RazorHotelDB25inClass.Models;

namespace RazorHotelDB25inClass.Helper
{
    public class RoomTypesCompare : IComparer<Room>
    {
        public int Compare(Room? x, Room? y)
        {
            if (x == null && y == null) { return 0; }
            else if (x == null) { return -1; }
            else if (y == null) { return 1; }
            return string.Compare(x.Types.ToString(), y.Types.ToString());
        }
    }
}
