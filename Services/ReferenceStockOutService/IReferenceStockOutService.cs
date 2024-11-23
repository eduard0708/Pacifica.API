namespace Pacifica.API.Services.ReferenceStockOutService
{
    public interface IReferenceStockOutService
    {
        Task<ApiResponse<IEnumerable<ReferenceStockOut>>> GetAllReferencesStockOutAsync();
        Task<ApiResponse<ReferenceStockOut>> GetReferenceStockOutByIdAsync(int id);
        Task<ApiResponse<ReferenceStockOut>> CreateReferenceStockOutAsync(ReferenceStockOut referenceStockOut);
        Task<ApiResponse<ReferenceStockOut>> UpdateReferenceStockOutAsync(int id, ReferenceStockOut referenceStockOut);
        Task<ApiResponse<bool>> DeleteReferenceStockOutAsync(int id);
    }
}