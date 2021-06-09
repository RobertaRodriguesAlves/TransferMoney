using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FundTransferService> _logger;

        public FundTransferService(ITransferMoneyProducerKafka transferMoneyProducerKafka,
                                    ITransferMoneyRepository repository,
                                    ILogger<FundTransferService> logger)
        {
            _transferMoneyProducerKafka = transferMoneyProducerKafka;
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> GetTransactionStatus(string transactionId)
        {
            _logger.LogInformation($"Searching for {transactionId} in the database");
            var transferEntity = await _repository.GetStatusAsync(transactionId);
            if (transferEntity == null)
                return new ResponseDto().Status = "Not Found";

            if (transferEntity.Status == null)
                return new ResponseDto().Status = "In Queue";
            
            if (transferEntity.Message == null)
                return new ResponseDto().Status = transferEntity.Status;

            return new FullResponseDto
            {
                Status = transferEntity.Status,
                Message = transferEntity.Message
            };
        }

        public async Task<string> Post(TransferDto transfer)
        {
            _logger.LogInformation($"Starting the post of transfer between accounts");
            var transferEntity = new TransferEntity
            {
                AccountOrigin = transfer.AccountOrigin,
                AccountDestination = transfer.AccountDestination,
                Value = transfer.Value
            };

            transferEntity.TransactionId = Guid.NewGuid();

            _logger.LogInformation("Inserting the transfer in the database");
            await _repository.InsertAsync(transferEntity);

            _logger.LogInformation("Sending the transfer to kafka topic");
            SendMessageToTopicKafka(transferEntity);

            _logger.LogInformation("Finishing the transfer processing");
            return transferEntity.TransactionId.ToString();
        }

        private void SendMessageToTopicKafka(TransferEntity transferEntity)
        {
            _transferMoneyProducerKafka.SendMessageToKafka(transferEntity);
        }
    }
}
