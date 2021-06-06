using System;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class FundTransferService : IFundTransferService
    {
        private readonly ITransferMoneyProducerKafka _transferMoneyProducerKafka;

        public FundTransferService(ITransferMoneyProducerKafka transferMoneyProducerKafka)
        {
            _transferMoneyProducerKafka = transferMoneyProducerKafka;
        }

        public Task<string> Get(string transactionId)
        {
            throw new System.NotImplementedException();
        }

        public string Post(TransferDto transfer)
        {
            var transferEntity = new TransferEntity
            {
                AccountOrigin = transfer.accountOrigin,
                AccountDestination = transfer.accountDestination,
                Value = transfer.value
            };

            transferEntity.TransactionId = Guid.NewGuid();
            _transferMoneyProducerKafka.StartAsync(transferEntity);
            return transferEntity.TransactionId.ToString();
        }
    }
}
