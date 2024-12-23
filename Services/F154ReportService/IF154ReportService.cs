using Pacifica.API.Dtos.F154Report;
using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Services.F154ReportService
{
    public interface IF154ReportService
    {
        // Get a specific DailySalesReport by Id
        Task<ApiResponse<F154SalesReportDto>> GetByIdAsync(int id);

        // Create a new DailySalesReport
        Task<ApiResponse<F154SalesReportDto>> CreateAsync(CreateF154SalesReportDto reportDto);

        // Update an existing DailySalesReport
        Task<ApiResponse<F154SalesReportDto>> UpdateAsync(F154SalesReportDto reportDto);

        // Delete a DailySalesReport by Id
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}