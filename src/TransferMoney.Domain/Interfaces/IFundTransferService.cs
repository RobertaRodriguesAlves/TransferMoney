using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces
{
    public interface IFundTransferService
    {
        string Post(TransferDto transfer);
        Task<string> Get(string transactionId);
    }
}
