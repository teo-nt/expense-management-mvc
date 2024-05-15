namespace ExpenseManagementMVC.Services.Exceptions
{
    public class NotAllowedActionException : Exception
    {
        public NotAllowedActionException(string s) : base(s) { }
    }
}
