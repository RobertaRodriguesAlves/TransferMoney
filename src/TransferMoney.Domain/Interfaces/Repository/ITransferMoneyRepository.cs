using System.Threading.Tasks;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces.Repository
{
    public interface ITransferMoneyRepository
    {
        Task InsertAsync(TransferEntity transfer);
        Task<TransferEntity> GetStatusAsync(string transactionId);
        Task<bool> UpdateTransactionInformationAsync(TransferEntity transfer);
    }
}
