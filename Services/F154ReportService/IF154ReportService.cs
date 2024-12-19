using Pacifica.API.Dtos.F154Report;
using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Services.F154ReportService
{
    public interface IF154ReportService
    {
        // Get a specific DailySalesReport by Id
        Task<ApiResponse<DailySalesReportDto>> GetByIdAsync(int id);

        // Create a new DailySalesReport
        Task<ApiResponse<DailySalesReportDto>> CreateAsync(DailySalesReportDto reportDto);

        // Update an existing DailySalesReport
        Task<ApiResponse<DailySalesReportDto>> UpdateAsync(DailySalesReportDto reportDto);

        // Delete a DailySalesReport by Id
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}