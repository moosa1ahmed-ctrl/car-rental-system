using System.ComponentModel.DataAnnotations;

namespace CarRental_CP317
{
    public class User
    {
        [Key]
        public int AccountID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class UserInformation
    {
        [Key]
        public int AccountID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PhoneNumber { get; set; }

    }
}