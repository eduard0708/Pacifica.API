using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Pacifica.API.Dtos.Employee;
using Pacifica.API.Dtos.Product;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UploadController(ApplicationDbContext context, IMapper mapper, UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;

            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("supplier-excel")]
        public async Task<IActionResult> UploadSupplierExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                var rowCount = worksheet.Dimension.Rows;

                var suppliers = new List<Supplier>();
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var supplier = new Supplier
                    {
                        SupplierName = worksheet.Cells[row, 1].Text, // Column 1: Supplier Name
                        ContactPerson = worksheet.Cells[row, 2].Text, // Column 2: Contact Person
                        ContactNumber = worksheet.Cells[row, 3].Text  // Column 3: Contact Number
                    };

                    // Optional: If Products are included, add logic here to populate them
                    suppliers.Add(supplier);
                }

                _context.Suppliers.AddRange(suppliers);
                await _context.SaveChangesAsync();

                return Ok("File processed and suppliers added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // New Method for Category Upload
        [HttpPost("category-excel")]
        public async Task<IActionResult> UploadCategoryExcel(IFormFile file)
        {


            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                var rowCount = worksheet.Dimension.Rows;

                var categories = new List<Category>();
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var category = new Category
                    {
                        CategoryName = worksheet.Cells[row, 1].Text, // Column 1: Category Name
                        Description = worksheet.Cells[row, 2].Text // Column 2: Description
                    };

                    categories.Add(category);
                }

                _context.Categories.AddRange(categories);
                await _context.SaveChangesAsync();

                return Ok("File processed and categories added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to upload the branch Excel file with only Branch Name and Location
        [HttpPost("branch-excel")]
        public async Task<IActionResult> UploadBranchExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                var rowCount = worksheet.Dimension.Rows;

                var branches = new List<Branch>();
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var branchName = worksheet.Cells[row, 1].Text; // Column 1: Branch Name
                    var branchLocation = worksheet.Cells[row, 2].Text; // Column 2: Branch Location

                    // Create a new branch with the extracted data
                    var branch = new Branch
                    {
                        BranchName = branchName,
                        BranchLocation = branchLocation,
                    };

                    branches.Add(branch);
                }

                // Add the branches to the database
                await _context.Branches.AddRangeAsync(branches);
                await _context.SaveChangesAsync();

                return Ok("File processed and branches added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Product/upload
        [HttpPost("product-excel")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> UploadProductsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "No file uploaded.",
                    Data = null
                });
            }

            // Check file type (ensure it's an Excel file)
            if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "Invalid file format. Please upload an Excel file.",
                    Data = null
                });
            }

            // Process the file and create products
            var products = await CreateProductsFromExcelAsync(file);

            if (products == null || !products.Any())
            {
                return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "No valid products found in the uploaded file.",
                    Data = null
                });
            }

            // Return success response with created products
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Message = $"{products.Count} products created successfully.",
                Data = products
            });
        }

        //this is the method will be call for Create products and asign to every branch
        // private async Task<List<ProductDto>> CreateProductsFromExcelAsync(IFormFile file)
        // {
        //     List<ProductDto> productDtos = new List<ProductDto>();

        //     using (var stream = new MemoryStream())
        //     {
        //         await file.CopyToAsync(stream);
        //         using (var package = new ExcelPackage(stream))
        //         {
        //             var worksheet = package.Workbook.Worksheets[0];

        //             // Assuming the first row contains headers
        //             int rowCount = worksheet.Dimension.Rows;

        //             // Get all branches (assuming you have a method to fetch all branches)
        //             var branches = await _context.Branches.ToListAsync();

        //             for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip the header
        //             {
        //                 string productName = worksheet.Cells[row, 1].Text.Trim();
        //                 string sku = worksheet.Cells[row, 2].Text.Trim();
        //                 int categoryId = int.TryParse(worksheet.Cells[row, 3].Text.Trim(), out var cId) ? cId : 0;
        //                 int supplierId = int.TryParse(worksheet.Cells[row, 4].Text.Trim(), out var sId) ? sId : 0;
        //                 string remarks = worksheet.Cells[row, 5].Text.Trim();
        //                 int statusId = int.TryParse(worksheet.Cells[row, 6].Text.Trim(), out var stId) ? stId : 0; // Capture StatusId

        //                 if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(sku) || categoryId == 0 || supplierId == 0 || statusId == 0)
        //                 {
        //                     continue; // Skip rows with missing essential data
        //                 }

        //                 var product = new Product
        //                 {
        //                     ProductName = productName,
        //                     SKU = sku,
        //                     CategoryId = categoryId,
        //                     SupplierId = supplierId,
        //                     Remarks = remarks,
        //                     CreatedBy = "admin",  // You may want to replace this with dynamic user data
        //                     CreatedAt = DateTime.Now
        //                 };

        //                 _context.Products.Add(product);
        //                 await _context.SaveChangesAsync();

        //                 // Create BranchProduct records for each branch
        //                 foreach (var branch in branches)
        //                 {
        //                     var branchProduct = new BranchProduct
        //                     {
        //                         BranchId = branch.Id,
        //                         ProductId = product.Id,
        //                         StatusId = statusId,  // Set the status
        //                         CostPrice = 0.00M,  // Default value (you can adjust this if necessary)
        //                         RetailPrice = 0.00M, // Default value (you can adjust this if necessary)
        //                         StockQuantity = 0,   // Default value (you can adjust this if necessary)
        //                         ReorderLevel = 0,    // Default value (you can adjust this if necessary)
        //                         MinStockLevel = 0,   // Default value (you can adjust this if necessary)
        //                         IsWeekly = false,    // Default value (you can adjust this if necessary)
        //                     };

        //                     _context.BranchProducts.Add(branchProduct);
        //                 }

        //                 await _context.SaveChangesAsync();

        //                 // Map product to DTO
        //                 var productDto = _mapper.Map<ProductDto>(product);
        //                 productDtos.Add(productDto);
        //             }
        //         }
        //     }

        //     return productDtos;
        // }

        private async Task<List<ProductDto>> CreateProductsFromExcelAsync(IFormFile file)
        {
            List<ProductDto> productDtos = new List<ProductDto>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    // Assuming the first row contains headers
                    int rowCount = worksheet.Dimension.Rows;

                    // Get all branches (assuming you have a method to fetch all branches)
                    var branches = await _context.Branches.ToListAsync();

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip the header
                    {
                        string productName = worksheet.Cells[row, 1].Text.Trim();
                        string sku = worksheet.Cells[row, 2].Text.Trim();
                        int categoryId = int.TryParse(worksheet.Cells[row, 3].Text.Trim(), out var cId) ? cId : 0;
                        int supplierId = int.TryParse(worksheet.Cells[row, 4].Text.Trim(), out var sId) ? sId : 0;
                        string remarks = worksheet.Cells[row, 5].Text.Trim();
                        int statusId = int.TryParse(worksheet.Cells[row, 6].Text.Trim(), out var stId) ? stId : 0; // Capture StatusId

                        if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(sku) || categoryId == 0 || supplierId == 0 || statusId == 0)
                        {
                            continue; // Skip rows with missing essential data
                        }

                        var product = new Product
                        {
                            ProductName = productName,
                            SKU = sku,
                            CategoryId = categoryId,
                            SupplierId = supplierId,
                            Remarks = remarks,
                            CreatedBy = "admin",  // You may want to replace this with dynamic user data
                            CreatedAt = DateTime.Now
                        };

                        _context.Products.Add(product);
                        await _context.SaveChangesAsync();

                        // Create BranchProduct records for each branch
                        foreach (var branch in branches)
                        {
                            var branchProduct = new BranchProduct
                            {
                                BranchId = branch.Id,
                                ProductId = product.Id,
                                StatusId = statusId,  // Set the status
                                CostPrice = 0.00M,  // Default value (you can adjust this if necessary)
                                RetailPrice = 0.00M, // Default value (you can adjust this if necessary)
                                StockQuantity = 0,   // Default value (you can adjust this if necessary)
                                ReorderLevel = 0,    // Default value (you can adjust this if necessary)
                                MinStockLevel = 0,   // Default value (you can adjust this if necessary)
                                IsWeekly = false,    // Default value (you can adjust this if necessary)
                            };

                            _context.BranchProducts.Add(branchProduct); // Add the BranchProduct to the context
                        }

                        // Save all branch products
                        await _context.SaveChangesAsync();

                        // Map product to DTO
                        var productDto = _mapper.Map<ProductDto>(product);
                        productDtos.Add(productDto);
                    }
                }
            }

            return productDtos;
        }

        // New Method to Upload Employee Excel
        [HttpPost("employee-excel")]
        public async Task<IActionResult> UploadEmployeeExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                var rowCount = worksheet.Dimension.Rows;

                var employees = new List<CreateEmployeeDto>();

                // Parsing the Excel rows
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var employee = new CreateEmployeeDto
                    {
                        EmployeeId = worksheet.Cells[row, 1].Text,
                        Username = worksheet.Cells[row, 2].Text,
                        FirstName = worksheet.Cells[row, 3].Text,
                        LastName = worksheet.Cells[row, 4].Text,
                        Email = worksheet.Cells[row, 5].Text,
                        Password = worksheet.Cells[row, 6].Text,
                        Department = int.TryParse(worksheet.Cells[row, 7].Text, out var deptId) ? deptId : 0,
                        Position = int.TryParse(worksheet.Cells[row, 8].Text, out var posId) ? posId : 0,
                        Roles = worksheet.Cells[row, 9].Text
                            .Split(',')
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => x.Trim())  // Trim roles
                            .ToList(),
                        Branches = worksheet.Cells[row, 10].Text
                            .Split(',')
                            .Where(x => int.TryParse(x.Trim(), out _))
                            .Select(int.Parse)
                            .ToList()
                    };

                    employees.Add(employee);
                }

                // Handle employee creation using Identity (UserManager)
                foreach (var createEmployeeDto in employees)
                {
                    var employee = new Employee
                    {
                        EmployeeId = createEmployeeDto.EmployeeId,
                        FirstName = createEmployeeDto.FirstName,
                        LastName = createEmployeeDto.LastName,
                        Email = createEmployeeDto.Email,
                        UserName = createEmployeeDto.Username, // Using username as the identity username
                        PositionId = createEmployeeDto.Position,
                        DepartmentId = createEmployeeDto.Department,
                        IsActive = true, // Default value for IsActive
                        IsDeleted = false, // Default value for IsDeleted
                        CreatedBy = "System" // Customize based on your requirement
                    };

                    // Fetch Department and Position from the context
                    if (createEmployeeDto.Department > 0)
                    {
                        var department = await _context.Departments.FindAsync(createEmployeeDto.Department);
                        if (department == null)
                        {
                            return BadRequest($"Department with ID {createEmployeeDto.Department} not found.");
                        }
                        employee.Department = department;
                    }

                    if (createEmployeeDto.Position > 0)
                    {
                        var position = await _context.Positions.FindAsync(createEmployeeDto.Position);
                        if (position == null)
                        {
                            return BadRequest($"Position with ID {createEmployeeDto.Position} not found.");
                        }
                        employee.Position = position;
                    }

                    // Create the employee (user) in Identity using UserManager
                    var result = await _userManager.CreateAsync(employee, createEmployeeDto.Password!);
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, $"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                    // Handle Roles mapping
                    foreach (var role in createEmployeeDto.Roles!)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                            if (!roleResult.Succeeded)
                            {
                                return StatusCode(500, $"Failed to create role: {role}");
                            }
                        }

                        await _userManager.AddToRoleAsync(employee, role);
                    }

                    // Handle Branches mapping
                    if (createEmployeeDto.Branches != null && createEmployeeDto.Branches.Any())
                    {
                        foreach (var branchId in createEmployeeDto.Branches)
                        {
                            var branch = await _context.Branches.FindAsync(branchId);
                            if (branch != null)
                            {
                                // Associate the employee with the branch in the EmployeeBranch table
                                var employeeBranch = new EmployeeBranch
                                {
                                    EmployeeId = employee.Id,  // Reference the newly created employee's ID
                                    BranchId = branchId,
                                    Role = "DefaultRole"  // You can replace with dynamic role assignment if needed
                                };
                                _context.EmployeeBranches.Add(employeeBranch);
                            }
                            else
                            {
                                return BadRequest($"Branch with ID {branchId} not found.");
                            }
                        }
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("File processed and employees added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("status-excel")]
        public async Task<IActionResult> UploadProductStatusExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                var rowCount = worksheet.Dimension.Rows;

                var statuses = new List<Status>();  // Assuming you have a model for Employee Statuses
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var status = new Status
                    {
                        StatusName = worksheet.Cells[row, 1].Text, // Column 1: Status Name
                        Description = worksheet.Cells[row, 2].Text // Column 2: Description
                    };

                    statuses.Add(status);
                }

                // Save statuses to the database
                _context.Statuses.AddRange(statuses);
                await _context.SaveChangesAsync();

                return Ok("File processed and employee statuses added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
