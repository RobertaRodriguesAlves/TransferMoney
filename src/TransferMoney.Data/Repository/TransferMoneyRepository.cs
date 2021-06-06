using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TransferMoney.Data.Context;
using TransferMoney.Domain.DTO;
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
        public async Task<bool> InsertAsync(TransferEntity transfer)
        {
            try
            {
                _logger.LogInformation($"Transfer's data: TransferId = {transfer.TransactionId}, " +
                    $"AccountOrigin = {transfer.AccountOrigin}, AccountDestination = {transfer.AccountDestination}, " +
                    $"Value = {transfer.Value}, StatusOfTheTransaction = {transfer.Status}, MessageAboutTheTransaction = {transfer.Message}");
                _dataSet.Add(transfer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Data was inserted in the database");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
                throw;
            }
        }

        public async Task<TransferEntity> GetStatus(string transactionId)
        {
            try
            {
                _logger.LogInformation($"Starting the searching for the transaction {transactionId} in the database");
                return await _dataSet.FirstOrDefaultAsync(operation => operation.TransactionId.ToString().Trim().Equals(transactionId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
                throw;
            }
        }
    }
}
