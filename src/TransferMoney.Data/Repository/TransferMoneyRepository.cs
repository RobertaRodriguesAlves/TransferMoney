using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TransferMoney.Data.Context;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces.Repository;

namespace TransferMoney.Data.Repository
{
    public class TransferMoneyRepository : ITransferMoneyRepository
    {
        protected readonly TransferMoneyDbContext _context;
        private readonly DbSet<TransferEntity> _dataSet;
        private readonly ILogger<TransferMoneyRepository> _logger;
        public TransferMoneyRepository(TransferMoneyDbContext context,
                                        ILogger<TransferMoneyRepository> logger) 
        {
            _context = context;
            _dataSet = context.Set<TransferEntity>();
            _logger = logger;
        }
        public async Task InsertAsync(TransferEntity transfer)
        {
            try
            {
                _logger.LogInformation($"Inserting the transactionId: {transfer.TransactionId} in the database");
                transfer.CreatedAt = DateTime.UtcNow;
                _dataSet.Add(transfer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Data was inserted in the database");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<TransferEntity> GetStatusAsync(string transactionId)
        {
            try
            {
                _logger.LogInformation($"Starting the searching for the transaction {transactionId} in the database");
                return await _dataSet.FirstOrDefaultAsync(operation => operation.TransactionId.ToString().Trim().Equals(transactionId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | Message: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateTransactionInformationAsync(TransferEntity transfer)
        {
            try
            {
                _logger.LogInformation(
                    $"Updating the Transfer's data in the database: TransferId = {transfer.TransactionId}, " +
                    $"AccountOrigin = {transfer.AccountOrigin}, " +
                    $"AccountDestination = {transfer.AccountDestination}, " +
                    $"Value = {transfer.Value}, StatusOfTheTransaction = {transfer.Status}," +
                    $" MessageAboutTheTransaction = {transfer.Message}");
                
                var result = await _dataSet.SingleOrDefaultAsync(transaction => transaction.TransactionId.ToString().Equals(transfer.TransactionId.ToString()));
                if (result == null)
                {
                    _logger.LogWarning($"The transactionId: {transfer.TransactionId} doesn't exists in the database");
                    return false;
                }
                   
                transfer.CreatedAt = result.CreatedAt;
                transfer.TransactionId = result.TransactionId;
                transfer.AccountDestination = result.AccountDestination;
                transfer.AccountOrigin = result.AccountOrigin;
                transfer.UpdatedAt = DateTime.UtcNow;
                _context.Entry(result).CurrentValues.SetValues(transfer);
                await _context.SaveChangesAsync();
                _logger.LogInformation("The transfer was updated");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
