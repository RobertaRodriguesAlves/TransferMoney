using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Domain.Interfaces.Repository;

namespace TranferMoney.KafkaConsumerApplication
{
    public class TransferMoneyConsumerKafka : ITransferMoneyConsumerKafka, IHostedService
    {
        private readonly ILogger<TransferMoneyConsumerKafka> _logger;
        private readonly IConsumer<Null, string> _consumer;
        private readonly IAccountInformationService _service;
        private readonly ITransferMoneyRepository _repository;

        public TransferMoneyConsumerKafka(ILogger<TransferMoneyConsumerKafka> logger,
                                             IAccountInformationService service, ITransferMoneyRepository repository)
        {
            _logger = logger;
            _service = service;
            _repository = repository;

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:9092",
                GroupId = nameof(TransferMoneyConsumerKafka),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listening the fund-transfer topic");
            _consumer.Subscribe("fund-transfer");
            try
            {
                while (true)
                {
                    var messageReceived = _consumer.Consume();
                    _logger.LogInformation($"Message received: {messageReceived.Message.Value} | Offset: {messageReceived.Offset}");
                    _logger.LogInformation("Starting the deserialize TransferEntity object");
                    var transferInformation = JsonConvert.DeserializeObject<TransferEntity>(messageReceived.Message.Value);
                    _logger.LogInformation($"Getting information of the transaction");
                    var result = await _service.MakesAccountOperation(transferInformation);
                    transferInformation.Status = result.Status;
                    transferInformation.Message = result.Message;
                    _logger.LogInformation($"Saving message in the database");
                    await _repository.InsertAsync(transferInformation);
                    _logger.LogInformation($"Message was saved");
                }
            }
            catch (Exception ex)
            {
                await StopAsync(cancellationToken);
                _logger.LogError($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
