using Microsoft.AspNetCore.Mvc;
using Pacifica.API.Dtos.F154Report;
using Pacifica.API.Models.Reports.F154Report;
using Pacifica.API.Services.F154ReportService;

namespace Pacifica.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class F154ReportController : ControllerBase
    {
        private readonly IF154ReportService _dailySalesReportService;

        public F154ReportController(IF154ReportService dailySalesReportService)
        {
            _dailySalesReportService = dailySalesReportService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DailySalesReportDto>>> GetByIdAsync(int id)
        {
            var response = await _dailySalesReportService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DailySalesReportDto>>> CreateAsync([FromBody]DailySalesReportDto reportDto)
        {
            var response = await _dailySalesReportService.CreateAsync(reportDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DailySalesReportDto>>> UpdateAsync(int id, DailySalesReportDto reportDto)
        {
            if (id != reportDto.Id)
            {
                return BadRequest(new ApiResponse<DailySalesReportDto>
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