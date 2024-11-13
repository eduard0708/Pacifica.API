using AutoMapper;
using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Category;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Dtos.Supplier;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Dtos.TransactionType;
using Pacifica.API.Dtos.Admin;

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

            CreateMap<TransactionType, TransactionTypeDto>().ReverseMap();
            CreateMap<TransactionTypeDto, TransactionType>();

            CreateMap<TransactionReference, TransactionReferenceDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Branch, BranchDto>().ReverseMap();
            CreateMap<Branch, CreateBranchDto>().ReverseMap();
            CreateMap<Branch, UpdateBranchDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
            CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();


            CreateMap<BranchProduct, BranchProductDto>();
            CreateMap<BranchProductDto, BranchProduct>();


        }
    }
}