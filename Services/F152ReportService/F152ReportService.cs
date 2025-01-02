
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.Reports.F152Report;
using Pacifica.API.Data;
using Pacifica.API.Dtos.F152Report;

namespace Pacifica.API.Services.F152ReportService
{
    public class F152ReportService : IF152ReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public F152ReportService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all transactions
        public async Task<IEnumerable<F152ReportTransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _context.F152ReportTransactions
                                              .Include(ft => ft.F152ReportCategories)
                                              .ToListAsync();

            return _mapper.Map<IEnumerable<F152ReportTransactionDto>>(transactions);
        }

        // Get a transaction by ID
        public async Task<F152ReportTransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.F152ReportTransactions
                                             .Include(ft => ft.F152ReportCategories)
                                             .FirstOrDefaultAsync(ft => ft.Id == id);

            return _mapper.Map<F152ReportTransactionDto>(transaction);
        }

        // Create a new transaction
        public async Task<F152ReportTransactionDto> CreateTransactionAsync(F152ReportTransactionDto transactionDto)
        {
            var transaction = _mapper.Map<F152ReportTransaction>(transactionDto);

            _context.F152ReportTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<F152ReportTransactionDto>(transaction);
        }

        // Update an existing transaction
        public async Task<bool> UpdateTransactionAsync(int id, F152ReportTransactionDto transactionDto)
        {
            var existingTransaction = await _context.F152ReportTransactions.FindAsync(id);
            if (existingTransaction == null)
                return false;

            _mapper.Map(transactionDto, existingTransaction);

            _context.F152ReportTransactions.Update(existingTransaction);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a transaction
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.F152ReportTransactions.FindAsync(id);
            if (transaction == null)
                return false;

            _context.F152ReportTransactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get categories by transaction ID
        public async Task<IEnumerable<F152ReportCategoryDto>> GetCategoriesByTransactionIdAsync(int transactionId)
        {
            var categories = await _context.F152ReportCategories
                                           .Where(fc => fc.F152ReportTransactionId == transactionId)
                                           .ToListAsync();

            return _mapper.Map<IEnumerable<F152ReportCategoryDto>>(categories);
        }

        // Create a new category
        public async Task<F152ReportCategoryDto> CreateCategoryAsync(int transactionId, F152ReportCategoryDto categoryDto)
        {
            var transaction = await _context.F152ReportTransactions.FindAsync(transactionId);
            if (transaction == null)
                return null!;

            var category = _mapper.Map<F152ReportCategory>(categoryDto);
            category.F152ReportTransactionId = transactionId;

            _context.F152ReportCategories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<F152ReportCategoryDto>(category);
        }

        // Update an existing category
        public async Task<bool> UpdateCategoryAsync(int id, F152ReportCategoryDto categoryDto)
        {
            var existingCategory = await _context.F152ReportCategories.FindAsync(id);
            if (existingCategory == null)
                return false;

            _mapper.Map(categoryDto, existingCategory);

            _context.F152ReportCategories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.F152ReportCategories.FindAsync(id);
            if (category == null)
                return false;

            _context.F152ReportCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


