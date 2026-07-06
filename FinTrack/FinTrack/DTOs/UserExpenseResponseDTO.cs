namespace AuthenticationServer.DTOs
{
    public class UserExpenseResponseDTO
    {
        public decimal TotalExpenseAmount { get; set; }
        public IEnumerable<UserExpenseDTO> Expenses { get; set; }
    }
}
