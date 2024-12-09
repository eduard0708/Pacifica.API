using Pacifica.API.Dtos.StockIn;

namespace Pacifica.API.Services.StockInService
{
    public interface IStockInService
    {
        Task<ApiResponse<IEnumerable<StockInDTO>>> GetAllStockInsAsync();
        Task<ApiResponse<StockInDTO>> GetStockInByIdAsync(int id);
        Task<ApiResponse<IEnumerable<StockInDTO>>> GetByReferenceNumber(string referenceNumber); 
        Task<ApiResponse<StockInDTO>> CreateStockInAsync(StockInCreateDTO stockInDto);
        Task<ApiResponse<IEnumerable<StockInDTO>>> CreateMultipleStockInAsync(IEnumerable<StockInCreateDTO> stockInDtos);
        Task<ApiResponse<StockInDTO>> UpdateStockInAsync(int id, StockInUpdateDTO stockInDto);
        Task<ApiResponse<bool>> DeleteStockInAsync(StockInDeleteParams deleteParams);
        Task<ApiResponse<bool>> RestoreStockInAsync(StockInRestoreParams restoreParams);
        Task<ApiResponse<List<StockIn>>> GetAllDeletedStockInAsync();
        Task<ApiResponse<IEnumerable<ViewStockInDTO>>> GetByDateRangeOrRefenceAsync(
            string referenceNumber, 
            DateTime? dateCreatedStart = null, 
            DateTime? dateCreatedEnd = null, 
            DateTime? dateReportedStart = null, 
            DateTime? dateReportedEnd = null);  

    }
}
