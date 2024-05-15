using AutoMapper;
using ExpenseManagementMVC.Repository;

namespace ExpenseManagementMVC.Services
{
    public class ApplicationService : IApplicationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _userLogger;
        private readonly ILogger<ExpenseService> _expenseLogger;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, ILogger<UserService> userLogger, ILogger<ExpenseService> expenseLogger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userLogger = userLogger;
            _expenseLogger = expenseLogger;
            _mapper = mapper;
        }

        public ExpenseService ExpenseService => new ExpenseService(_unitOfWork, _expenseLogger, _mapper);

        public UserService UserService => new UserService(_userLogger, _mapper, _unitOfWork);
    }
}
