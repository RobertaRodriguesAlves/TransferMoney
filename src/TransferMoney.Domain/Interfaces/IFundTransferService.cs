using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Domain.Interfaces
{
    public interface IFundTransferService
    {
        Task<bool> Post(TransferDto transfer);
        Task<string> Get(string transactionId);
    }
}
