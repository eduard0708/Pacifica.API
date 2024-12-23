using Microsoft.EntityFrameworkCore;
using Pacifica.API.Dtos.F154Report;
using Pacifica.API.Models.Reports.F154Report;
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Services.F154ReportService
{
    public class F154ReportService : IF154ReportService
    {
        private readonly ApplicationDbContext _context;

        public F154ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<F154SalesReportDto>> GetByIdAsync(int id)
        {
            var response = new ApiResponse<F154SalesReportDto>();

            var F154SalesReport = await _context.F154SalesReports
                .Include(dsr => dsr.Branch)
                .Include(dsr => dsr.CashDenominations)
                .Include(dsr => dsr.SalesBreakdowns)
                .Include(dsr => dsr.Checks)
                .FirstOrDefaultAsync(dsr => dsr.Id == id);

            if (F154SalesReport == null)
            {
                response.Success = false;
                response.Message = "Daily Sales Report not found.";
                return response;
            }

            // Map entity to DTO
            response.Success = true;
            response.Message = "Daily Sales Report retrieved successfully.";
            response.Data = MapToDto(F154SalesReport);

            return response;
        }

        public async Task<ApiResponse<F154SalesReport>> CreateAsync(CreateF154SalesReportDto reportDto)
        {
            var response = new ApiResponse<F154SalesReport>();

            try
            {
                // Map DTO to Entity
                var report = MapToEntity(reportDto);

                _context.F154SalesReports.Add(report);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Daily Sales Report created successfully.";
                // response.Data = MapToDto(report);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating Daily Sales Report: {ex.Message}";
            }

            return response;
        }

        public async Task<ApiResponse<F154SalesReportDto>> UpdateAsync(F154SalesReportDto reportDto)
        {
            var response = new ApiResponse<F154SalesReportDto>();

            var existingReport = await _context.F154SalesReports.FindAsync(reportDto.Id);
            if (existingReport == null)
            {
                response.Success = false;
                response.Message = "Daily Sales Report not found.";
                return response;
            }

            // Map DTO to Entity and update
            existingReport.dateReported = reportDto.dateReported;
            existingReport.SalesForTheDay = reportDto.SalesForTheDay;
            existingReport.GrossSalesCRM = reportDto.GrossSalesCRM;
            // Update other fields as needed...

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Daily Sales Report updated successfully.";
            response.Data = MapToDto(existingReport);

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var response = new ApiResponse<bool>();

            var report = await _context.F154SalesReports.FindAsync(id);
            if (report == null)
            {
                response.Success = false;
                response.Message = "Daily Sales Report not found.";
                return response;
            }

            _context.F154SalesReports.Remove(report);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Daily Sales Report deleted successfully.";
            response.Data = true;

            return response;
        }

        // Helper method to map entity to DTO
        private F154SalesReportDto MapToDto(F154SalesReport entity)
        {
            return new F154SalesReportDto
            {
                Id = entity.Id,
                dateReported = entity.dateReported,
                BranchId = entity.BranchId,
                BranchName = entity.Branch!.BranchName,
                SalesForTheDay = entity.SalesForTheDay,
                GrossSalesCRM = entity.GrossSalesCRM,
                GrossSalesCashSlip = entity.GrossSalesCashSlip,
                TotalSales = entity.OverAllTotal,
                NetAccountability = entity.NetAccountability,
                CashSlip = entity.CashSlip,
                ChargeInvoice = entity.ChargeInvoice,
                PaymentsOfAccounts = entity.PaymentsOfAccounts,
                OtherReceipts = entity.OtherReceipts,
                CertifiedBy = entity.CertifiedBy,
                ApprovedBy = entity.ApprovedBy,
                // Map related data

                CashDenominations = entity.CashDenominations!.Select(c => new CashDenominationDto
                {
                    Id = c.Id,
                    Denomination = (DenominationEnums)c.Denomination!,
                    Quantity = c.Quantity,
                    Amount = c.Amount
                }).ToList(),

                SalesBreakdowns = entity.SalesBreakdowns!.Select(s => new SalesBreakdownDto
                {
                    Id = s.Id,
                    ProductCategory = s.ProductCategory,
                    Amount = s.Amount
                }).ToList(),
                Checks = entity.Checks!.Select(c => new CheckDto
                {
                    Id = c.Id,
                    Maker = c.Maker,
                    Bank = c.Bank,
                    CheckNumber = c.CheckNumber,
                    Amount = c.Amount
                }).ToList()
            };
        }

        // Helper method to map DTO to Entity
        private F154SalesReport MapToEntity(CreateF154SalesReportDto dto)
        {
            return new F154SalesReport
            {
                // Id = dto.Id,
                dateReported = dto.dateReported,
                BranchId = dto.BranchId,
                SalesForTheDay = dto.SalesForTheDay,
                GrossSalesCRM = dto.GrossSalesCRM,
                GrossSalesCashSlip = dto.GrossSalesCashSlip,
                NetAccountability = dto.NetAccountability,
                CashSlip = dto.CashSlip,
                ChargeInvoice = dto.ChargeInvoice,
                PaymentsOfAccounts = dto.PaymentsOfAccounts,
                OtherReceipts = dto.OtherReceipts,
                CertifiedBy = dto.CertifiedBy,
                ApprovedBy = dto.ApprovedBy,
                // Map related data
                CashDenominations = dto.CashDenominations!.Select(c => new CashDenomination
                {
                    Denomination = c.CashDenomination,  // Convert the int to the enum
                    Quantity = c.Quantity,
                    Amount = c.Amount
                }).ToList(),

                SalesBreakdowns = dto.SalesBreakdowns!.Select(s => new SalesBreakdown
                {
             
                    // Mapping the Description property (which could be a ProductCategory in the SalesBreakdown model)
                    ProductCategory = s.ProductCategory,
                    Amount = s.Amount // Mapping the Amount property from DTO to Entity
                }).ToList(),

                Checks = dto.Checks!.Select(c => new Check
                {
                    Maker= c.Maker,
                    Bank = c.Bank,
                    CheckNumber = c.CheckNumber,
                    Amount = c.Amount // Mapping check-related properties
                }).ToList(),

                Less = new Less
                {
                    OverPunch = dto.Less!.OverPunch,
                    SalesReturnOP = dto.Less.SalesReturnOP,
                    ChargeSales = dto.Less.ChargeSales
                }

            };
        }

    
    }
}
