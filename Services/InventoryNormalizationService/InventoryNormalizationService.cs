using Microsoft.EntityFrameworkCore;
using Pacifica.API.Data;
using Pacifica.API.Dtos.InventoryNormalization;
using Pacifica.API.Models.Inventory;


namespace Pacifica.API.Services.InventoryNormalizationService{
    public class InventoryNormalizationService : IInventoryNormalizationService
    {
        private readonly ApplicationDbContext _context;

        public InventoryNormalizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryNormalization>> GetAllAsync()
        {
            return await _context.InventoryNormalizations
                                 .Include(n => n.Inventory)
                                 .ToListAsync();
        }

        public async Task<InventoryNormalization?> GetByIdAsync(int id)
        {
            return await _context.InventoryNormalizations
                                 .Include(n => n.Inventory)
                                 .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<InventoryNormalization> CreateAsync(InventoryNormalizationDto dto)
        {
            var normalization = new InventoryNormalization
            {
                InventoryId = dto.InventoryId,
                AdjustedQuantity = dto.AdjustedQuantity,
                NormalizationDate = dto.NormalizationDate
            };

            _context.InventoryNormalizations.Add(normalization);
            await _context.SaveChangesAsync();

            return normalization;
        }

        public async Task<InventoryNormalization?> UpdateAsync(int id, InventoryNormalizationDto dto)
        {
            var normalization = await _context.InventoryNormalizations.FindAsync(id);
            if (normalization == null)
                return null;

            normalization.AdjustedQuantity = dto.AdjustedQuantity;
            normalization.NormalizationDate = dto.NormalizationDate;
            normalization.InventoryId = dto.InventoryId;

            _context.InventoryNormalizations.Update(normalization);
            await _context.SaveChangesAsync();

            return normalization;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var normalization = await _context.InventoryNormalizations.FindAsync(id);
            if (normalization == null)
                return false;

            _context.InventoryNormalizations.Remove(normalization);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}




