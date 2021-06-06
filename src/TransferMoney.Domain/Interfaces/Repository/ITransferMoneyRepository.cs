using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces.Repository
{
    public interface ITransferMoneyRepository
    {
        Task<bool> InsertAsync(TransferEntity item);
        Task<TransferEntity> GetStatus(string transactionId);
    }
}
