using System.Threading;
using System.Threading.Tasks;

namespace TransferMoney.Domain.Interfaces
{
    public interface ITransferMoneyConsumerKafka
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
