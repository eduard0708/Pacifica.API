using AutoMapper;
using PacificaAPI.Dtos.Admin;

namespace Pacifica.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<EmployeeProfile, EmployeeProfileDto>().ReverseMap();
            CreateMap<LoginDto, Employee>().ReverseMap();
            CreateMap<RegisterDto, Employee>().ReverseMap();
        }
    }
}