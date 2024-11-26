using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pacifica.API.Dtos.StockIn;

namespace Pacifica.API.Services.StockInService
{
    public interface IStockInService
    {
        Task<ApiResponse<IEnumerable<StockInDTO>>> GetAllStockInsAsync();
        Task<ApiResponse<StockInDTO>> GetStockInByIdAsync(int id);
        Task<ApiResponse<IEnumerable<StockInDTO>>> GetStockInByReferenceNumberAsync(string referenceNumber);  // New method to get by reference number
        Task<ApiResponse<StockInDTO>> CreateStockInAsync(StockInCreateDTO stockInDto);
        Task<ApiResponse<IEnumerable<StockInDTO>>> CreateMultipleStockInAsync(IEnumerable<StockInCreateDTO> stockInDtos);
        Task<ApiResponse<StockInDTO>> UpdateStockInAsync(int id, StockInUpdateDTO stockInDto);
         Task<ApiResponse<bool>> DeleteStockInAsync(int id);
    }
}
