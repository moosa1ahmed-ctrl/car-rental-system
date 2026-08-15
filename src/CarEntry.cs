using System.ComponentModel.DataAnnotations;

namespace CarRental_CP317
{
    public class CarEntry
    {
        [Key]
        public int CarID { get; set; }
        public int CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int[] BookingIDs { get; set; } = Array.Empty<int>(); // array of booking IDs associated with this car, the Bookings in the array should be checked to see if the car is available at a certain time range
        
        // public bool IsAvailable { get; set; } OLD, replaced with BookingIDs
        // public string AvailabilityDate { get; set; } // OLD, replaced with BookingIDs - the date when the car becomes available if it is not currently available
    }
}