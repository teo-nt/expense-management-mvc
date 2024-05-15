using AutoMapper;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;

namespace ExpenseManagementMVC.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ExpenseInsertDTO, Expense>().ReverseMap();
            CreateMap<ExpenseUpdateDTO, Expense>().ReverseMap();
            CreateMap<ExpenseReadOnlyDTO, Expense>().ReverseMap();

            CreateMap<UserLoginDTO, User>().ReverseMap();
            CreateMap<UserSignUpDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<UserUpdatePasswordDTO, User>();
        }
    }
}
