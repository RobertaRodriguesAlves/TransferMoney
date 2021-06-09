using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Domain.Interfaces
{
    public interface IFundTransferService
    {
        Task<string> Post(TransferDto transfer);
        Task<object> GetTransactionStatus(string transactionId);
    }
}
