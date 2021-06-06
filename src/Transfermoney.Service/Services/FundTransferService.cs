using System;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Domain.Interfaces.Repository;

namespace TransferMoney.Service.Services
{
    public class FundTransferService : IFundTransferService
    {
        private readonly ITransferMoneyProducerKafka _transferMoneyProducerKafka;
        private readonly ITransferMoneyRepository _repository;

        public FundTransferService(ITransferMoneyProducerKafka transferMoneyProducerKafka,
                                    ITransferMoneyRepository repository)
        {
            _transferMoneyProducerKafka = transferMoneyProducerKafka;
            _repository = repository;
        }

        public async Task<object> Get(string transactionId)
        {
            var transferEntity = await _repository.GetStatus(transactionId);
            if (transferEntity == null)
                return new ResponseDto().Status = "In Queue";
            
            if (transferEntity.Message == null)
                return new ResponseDto().Status = transferEntity.Status;

            return new FullResponseDto
            {
                Status = transferEntity.Status,
                Message = transferEntity.Message
            };
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
            _transferMoneyProducerKafka.SendMessageToKafka(transferEntity);
            return transferEntity.TransactionId.ToString();
        }
    }
}
