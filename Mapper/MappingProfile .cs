using Pacifica.API.Dtos.Branch;
using Pacifica.API.Dtos.BranchProduct;
using Pacifica.API.Dtos.Category;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Dtos.Supplier;
using Pacifica.API.Dtos.TransactionReference;
using Pacifica.API.Dtos.Admin;
using Pacifica.API.Dtos.AuditTrails;
using Pacifica.API.Models.GlobalAuditTrails;
using Pacifica.API.Dtos.StockIn;
using Pacifica.API.Models.Transaction;
using Pacifica.API.Dtos.StockInReference;
using Pacifica.API.Dtos.StockOut;
using Pacifica.API.Dtos.StockOutReference;
using Pacifica.API.Models.Inventory;
using Pacifica.API.Dtos.Inventory;
using Pacifica.API.Dtos.PaymentMethod;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Models.EmployeManagement;
using Pacifica.API.Dtos.UserManagement;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Dtos.Menu;
using Pacifica.API.Models.Menu;
using Pacifica.API.Dtos.Status;
using Pacifica.API.Dtos.Role;
using Pacifica.API.Models.Reports.F152Report;
using Pacifica.API.Dtos.F152Report;

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


                        CreateMap<TransactionReference, TransactionReferenceDto>().ReverseMap();
                        CreateMap<TransactionReference, CreateTransactionReferenceDto>().ReverseMap();
                        CreateMap<TransactionReference, UpdateTransactionReferenceDto>().ReverseMap();

                        CreateMap<TransactionType, TransactionTypeDto>().ReverseMap();
                        CreateMap<TransactionType, CreateTransactionTypeDto>().ReverseMap();
                        CreateMap<TransactionType, UpdateTransactionTypeDto>().ReverseMap();

                        CreateMap<Status, StatusDto>().ReverseMap();
                        CreateMap<Status, UpdateStatusDto>().ReverseMap();
                        CreateMap<Status, CreateStatusDto>().ReverseMap();

                        CreateMap<Category, CategoryDto>().ReverseMap();
                        CreateMap<Category, UpdateCategoryDto>().ReverseMap();
                        CreateMap<Category, CreateCategoryDto>().ReverseMap();
                        CreateMap<Category, SelectCategoryDTO>().ReverseMap();


                        CreateMap<Branch, BranchDto>().ReverseMap();
                        CreateMap<Branch, CreateBranchDto>().ReverseMap();
                        CreateMap<Branch, UpdateBranchDto>().ReverseMap();
                        CreateMap<Branch, BranchProduct_BranchDto>().ReverseMap();


                        CreateMap<Product, ProductDto>()
                                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.CategoryName))
                                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier!.SupplierName));

                        // Configure the mapping between Product and SelectSupplierDTO
                        // Map Supplier entity to SelectSupplierDTO
                        CreateMap<Supplier, SelectSupplierDTO>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName));


                        CreateMap<Product, CreateProductDto>().ReverseMap();
                        CreateMap<Product, UpdateProductDto>().ReverseMap();
                        CreateMap<Product, DeletetedProductsDto>().ReverseMap();

                        CreateMap<Supplier, SupplierDto>().ReverseMap();
                        CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
                        CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();

                        CreateMap<BranchProduct, BranchProductDto>().ReverseMap();
                        CreateMap<BranchProduct, AddProductToBranchDto>().ReverseMap();
                        CreateMap<BranchProduct, UpdateBranchProductDto>().ReverseMap();
                        CreateMap<BranchProduct, UpdateBranchProductDto>().ReverseMap();

                        CreateMap<BranchProduct, BranchProductResponseDto>()
                                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.BranchName))
                                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
                                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Product!.CategoryId))
                                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product!.Category!.CategoryName))
                                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Product!.SupplierId))
                                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Product!.Supplier!.SupplierName))
                                .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product!.SKU))
                                .ForMember(dest => dest.MinStockLevel, opt => opt.MapFrom(src => src.MinStockLevel))
                                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel))
                                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status!.StatusName))
                                .ReverseMap();

                        CreateMap<StockIn, StockInDTO>().ReverseMap();
                        CreateMap<StockIn, StockInCreateDTO>().ReverseMap();
                        CreateMap<StockIn, StockInUpdateDTO>().ReverseMap();
                        CreateMap<StockIn, StockInDTO>().ReverseMap();

                        CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();

                        CreateMap<StockInReference, StockInReferenceDto>().ReverseMap();
                        CreateMap<StockInReference, CreateStockInReferenceDto>().ReverseMap();
                        CreateMap<StockInReference, UpdateStockInReferenceDto>().ReverseMap();
                        CreateMap<StockInReference, SelectReferenceStockInDTO>().ReverseMap();

                        CreateMap<StockOut, StockOutDTO>().ReverseMap();
                        CreateMap<StockOut, CreateStockOutDTO>().ReverseMap();


                        CreateMap<StockOutReference, StockOutReferenceDto>().ReverseMap();
                        CreateMap<StockOutReference, CreateStockOutReferenceDto>().ReverseMap();
                        CreateMap<StockOutReference, UpdateStockOutReferenceDto>().ReverseMap();
                        CreateMap<StockOutReference, SelectReferenceStockOutDTO>().ReverseMap();

                        // Map ProductAuditTrail to ProductAuditTrailsDto
                        CreateMap<ProductAuditTrail, ProductAuditTrailsDto>()
                                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
                                .ForMember(dest => dest.NewValue, opt => opt.MapFrom(src => src.NewValue))
                                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                                .ForMember(dest => dest.ActionBy, opt => opt.MapFrom(src => src.ActionBy))
                                .ForMember(dest => dest.ActionDate, opt => opt.MapFrom(src => src.ActionDate)).ReverseMap();


                        CreateMap<BranchProductAuditTrail, BranchProductAuditTrailsDto>()
                                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
                                .ForMember(dest => dest.NewValue, opt => opt.MapFrom(src => src.NewValue))
                                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                                .ForMember(dest => dest.ActionBy, opt => opt.MapFrom(src => src.ActionBy))
                                .ForMember(dest => dest.ActionDate, opt => opt.MapFrom(src => src.ActionDate)).ReverseMap();


                        CreateMap<Inventory, CreateInventoryDTO>().ReverseMap();

                        CreateMap<CreateMenuDto, Menu>();


                        CreateMap<Menu, MenuDto>().ReverseMap();

                        CreateMap<Inventory, ResponseInventoryDTO>()
                                .ForMember(dest => dest.InventoryDate, opt => opt.MapFrom(src => src.InventoryDate))
                                .ReverseMap();

                        CreateMap<InventoryNormalization, InventoryNormalizationDto>().ReverseMap();


                        // Create mappings between your models and DTOs
                        CreateMap<BeginningInventory, BeginningInventoryDto>().ReverseMap();
                        CreateMap<BeginningInventory, CreateBeginningInventoryDto>().ReverseMap();
                        CreateMap<BeginningInventory, UpdateBeginningInventoryDto>().ReverseMap();

                        CreateMap<Department, DepartmentDto>().ReverseMap();
                        CreateMap<Position, PositionDto>().ReverseMap();
                        // Map Employee to GetEmployeeDto

                        CreateMap<Employee, EmployeeDto>()
                            // Map EmployeeId correctly from Employee to EmployeeDto
                            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                            // Convert Guid to string for Id if EmployeeId is a Guid type
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))  // Convert Guid to string

                            // Mapping for Department
                            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? new List<string> { src.Department.Name! } : new List<string>()))

                            // Mapping for Roles
                            //     .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles != null ? src.Roles.Select(role => role.Name!).ToList() : new List<string>()))

                            // Mapping for Position
                            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position != null ? new List<string> { src.Position.Name! } : new List<string>())); // Null check for Position



                        // Map from F152ReportTransaction to F152ReportTransactionDto
                        CreateMap<F152ReportTransaction, F152ReportTransactionDto>()
                            .ForMember(dest => dest.F152ReportCategories, opt => opt.MapFrom(src => src.F152ReportCategories));

                        // Map from F152ReportCategory to F152ReportCategoryDto
                        CreateMap<F152ReportCategory, F152ReportCategoryDto>().ReverseMap();
                }
        }
}

