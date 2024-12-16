using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Models;
using Pacifica.API.Models.EmployeManagement;
using System.Threading.Tasks;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/assign")]
    public class AssignEmployeeToBranchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignEmployeeToBranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/assign/employee-to-branch
        [HttpPost("employee-to-branch")]
        public async Task<IActionResult> AssignEmployeeToBranch([FromBody] AssignEmployeeToBranchRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            // Check if the employee exists
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {request.EmployeeId} not found.");
            }

            // Check if the branch exists
            var branch = await _context.Branches.FindAsync(request.BranchId);
            if (branch == null)
            {
                return NotFound($"Branch with ID {request.BranchId} not found.");
            }

            // Create an EmployeeBranch object
            var employeeBranch = new EmployeeBranch
            {
                EmployeeId = request.EmployeeId,
                BranchId = request.BranchId
            };

            // Add the EmployeeBranch to the database
            _context.EmployeeBranches.Add(employeeBranch);
            await _context.SaveChangesAsync();

            return Ok("Employee successfully assigned to the branch.");
        }
    }

    // DTO for assigning employee to branch
    public class AssignEmployeeToBranchRequest
    {
        public string? EmployeeId { get; set; }  // Employee's ID
        public int BranchId { get; set; }       // Branch's ID
    }
}
