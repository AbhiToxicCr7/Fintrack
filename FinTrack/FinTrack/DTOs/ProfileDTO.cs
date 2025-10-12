namespace FinTrack.DTOs
{
    //To get the profile information in case of a GET request
    public class ProfileDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
