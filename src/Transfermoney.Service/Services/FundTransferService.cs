using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class FundTransferService : IFundTransferService
    {
        private readonly IAccountInformationService _accountInformationService;
        private readonly ITransferMoneyProducerKafka _transferMoneyProducerKafka;

        public FundTransferService(IAccountInformationService accountInformationService,
                                    ITransferMoneyProducerKafka transferMoneyProducerKafka)
        {
            _accountInformationService = accountInformationService;
            _transferMoneyProducerKafka = transferMoneyProducerKafka;
        }

        public Task<string> Get(string transactionId)
        {
            throw new System.NotImplementedException();
        }

        public bool Post(TransferEntity transfer)
        {
            _transferMoneyProducerKafka.StartAsync(transfer);
            return true;
        }
    }
}
