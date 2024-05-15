namespace ExpenseManagementMVC.DTO
{
    public class ExpenseUpdateDTO : BaseDTO
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public DateTime Date { get; set; }
    }
}
