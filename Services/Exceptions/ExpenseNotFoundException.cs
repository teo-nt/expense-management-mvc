namespace ExpenseManagementMVC.Services.Exceptions
{
    public class ExpenseNotFoundException : Exception
    {
        public ExpenseNotFoundException(string s) : base(s) { }
    }
}
