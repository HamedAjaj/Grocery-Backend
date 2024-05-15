namespace Grocery.Domain.Entities.Identity
{
    public class Address
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AppUserId { get; set; }  // Foriegn key of user
        public AppUser User { get; set; } // Navigation prop [One]
    }
}