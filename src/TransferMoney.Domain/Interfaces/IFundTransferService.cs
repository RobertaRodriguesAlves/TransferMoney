using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces
{
    public interface IFundTransferService
    {
        bool Post(TransferEntity transfer);
        Task<string> Get(string transactionId);
    }
}
