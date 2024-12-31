using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Pacifica.API.Dtos.Product;
using Pacifica.API.Models;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UploadController(ApplicationDbContext context)
        {
            _context = context;
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

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip the header
                    {
                        string productName = worksheet.Cells[row, 1].Text.Trim();
                        string sku = worksheet.Cells[row, 2].Text.Trim();
                        int categoryId = int.TryParse(worksheet.Cells[row, 3].Text.Trim(), out var cId) ? cId : 0;
                        int supplierId = int.TryParse(worksheet.Cells[row, 4].Text.Trim(), out var sId) ? sId : 0;
                        string remarks = worksheet.Cells[row, 5].Text.Trim();

                        if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(sku) || categoryId == 0 || supplierId == 0)
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

                        // Map product to DTO
                        var productDto = _mapper.Map<ProductDto>(product);
                        productDtos.Add(productDto);
                    }
                }
            }

            return productDtos;
        }
    }
}
