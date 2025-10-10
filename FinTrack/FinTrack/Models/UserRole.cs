namespace FinTrack.Models
{
    //Establishing a many to many relationship between User and Role
    public class UserRole
    {
        //FK representing user
        public int UserId { get; set; }

        //FK representing role
        public int RoleId { get; set; }

        //Navigation property to user
        public User User { get; set; }

        //Navigation property to Role
        public Role Role { get; set; }
    }
}
