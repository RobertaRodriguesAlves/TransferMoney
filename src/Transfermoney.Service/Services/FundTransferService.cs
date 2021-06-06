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

        public async Task<ResponseDto> Get(string transactionId)
        {
            var response = new ResponseDto();
            var transferEntity = await _repository.GetStatus(transactionId);
            if (transferEntity == null)
            {
                response.Status = "In Queue";
            }
            else
            {
                response.Status = transferEntity.Status;
                response.Message = transferEntity.Message;
            }
            return response;

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
