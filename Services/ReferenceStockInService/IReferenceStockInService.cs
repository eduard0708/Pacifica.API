namespace Pacifica.API.Services.ReferenceStockInService
{
    public interface IReferenceStockInService
    {
        Task<ApiResponse<IEnumerable<ReferenceStockIn>>> GetAllReferencesStockInAsync();
        Task<ApiResponse<ReferenceStockIn>> GetReferencesStockInByIdAsync(int id);
        Task<ApiResponse<ReferenceStockIn>> CreateReferenceStockInAsync(ReferenceStockIn ReferenceStockIn);
        Task<ApiResponse<ReferenceStockIn>> UpdateReferenceStockInAsync(int id, ReferenceStockIn ReferenceStockIn);
        Task<ApiResponse<bool>> DeleteReferenceStockInAsync(int id);
    }
}