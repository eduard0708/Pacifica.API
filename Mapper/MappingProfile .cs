using AutoMapper;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Dtos.TransactionType;
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

            CreateMap<TransactionType, TransactionTypeDto>();
            CreateMap<TransactionTypeDto, TransactionType>();

            CreateMap<TransactionReference, TransactionReferenceDto>();
            CreateMap<TransactionReferenceDto, TransactionReference>();
        }
    }
}