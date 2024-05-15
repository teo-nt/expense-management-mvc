namespace ExpenseManagementMVC.Services
{
    public interface IApplicationsService
    {
        ExpenseService ExpenseService { get; }
        UserService UserService { get; }
    }
}
