using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Domain.Interfaces
{
    public interface ITransferMoneyProducerKafka
    {
        Task StartAsync(TransferDto transfer);
    }
}
