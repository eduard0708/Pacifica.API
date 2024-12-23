using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.F154Report;
using Pacifica.API.Models.Reports.F154Report;
using Pacifica.API.Services.F154ReportService;

namespace Pacifica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class F154ReportController : ControllerBase
    {
        private readonly IF154ReportService _dailySalesReportService;

        public F154ReportController(IF154ReportService dailySalesReportService)
        {
            _dailySalesReportService = dailySalesReportService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<F154SalesReportDto>>> GetByIdAsync(int id)
        {
            var response = await _dailySalesReportService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<F154SalesReport>>> CreateAsync([FromBody] CreateF154SalesReportDto reportDto)
        {
            var response = await _dailySalesReportService.CreateAsync(reportDto);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<F154SalesReport>
                {
                    Success = false,
                    Message = $"Failed to create product: {response.Message}",
                    Data = null
                });
            }

            return Ok(new ApiResponse<F154SalesReport>
            {
                Success = true,
                Message = "Products created successfully.",
                Data = response.Data
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<F154SalesReportDto>>> UpdateAsync(int id, F154SalesReportDto reportDto)
        {
            if (id != reportDto.Id)
            {
                return BadRequest(new ApiResponse<F154SalesReportDto>
                {
                    Success = false,
                    Message = "Report ID mismatch."
                });
            }

            var response = await _dailySalesReportService.UpdateAsync(reportDto);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync(int id)
        {
            var response = await _dailySalesReportService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }

}