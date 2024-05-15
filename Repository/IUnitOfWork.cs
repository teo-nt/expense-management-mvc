namespace ExpenseManagementMVC.Repository
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        ExpenseRepository ExpenseRepository { get; }

        Task<bool> SaveAsync();
    }
}
