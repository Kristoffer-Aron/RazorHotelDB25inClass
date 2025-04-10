using System.ComponentModel.DataAnnotations;

namespace RazorHotelDB25inClass.Models
{
    public class Hotel : IComparable<Hotel>
    {
        [Required(ErrorMessage = "HotelNr is required")]
        public int HotelNr { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Exceeded character length of 30")]
        public String Navn { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Exceeded character length of 50")]
        public String Adresse { get; set; }

        public Hotel()
        {
        }

        public Hotel(int hotelNr, string navn, string adresse)
        {
            HotelNr = hotelNr;
            Navn = navn;
            Adresse = adresse;
        }

        public int CompareTo(Hotel? other)
        {
            return Navn.CompareTo(other.Navn);
        }

        public override string ToString()
        {
            return $"{nameof(HotelNr)}: {HotelNr}, {nameof(Navn)}: {Navn}, {nameof(Adresse)}: {Adresse}";
        }
    }
}
