namespace RSpot.Users.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
        public string Name { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
    }

}
