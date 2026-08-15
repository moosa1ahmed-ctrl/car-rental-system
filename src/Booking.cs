using System.ComponentModel.DataAnnotations;

namespace CarRental_CP317
{
    // unused for now
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int AccountID { get; set; }
        public int CarID { get; set; }
        public int TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}