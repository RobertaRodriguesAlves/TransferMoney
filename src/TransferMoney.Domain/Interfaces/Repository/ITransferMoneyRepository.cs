using System.Threading.Tasks;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces.Repository
{
    public interface ITransferMoneyRepository
    {
        Task<bool> InsertAsync(TransferEntity item);
    }
}
