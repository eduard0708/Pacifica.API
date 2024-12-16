using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
    
    
    
     // Endpoint to upload the branch Excel file
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
    }
}
