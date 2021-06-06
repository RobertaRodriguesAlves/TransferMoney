using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces
{
    public interface ITransferMoneyProducerKafka
    {
        Task SendMessageToKafka(TransferEntity transfer);
    }
}
