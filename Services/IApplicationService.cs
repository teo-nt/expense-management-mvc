namespace ExpenseManagementMVC.Services
{
    public interface IApplicationService
    {
        ExpenseService ExpenseService { get; }
        UserService UserService { get; }
    }
}
