using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class TransferMoneyKafkaProducer : ITransferMoneyProducerKafka
    {
        private readonly ILogger<TransferMoneyKafkaProducer> _logger;
        private readonly IProducer<Null, string> _producer;
        public TransferMoneyKafkaProducer(ILogger<TransferMoneyKafkaProducer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "127.0.0.1:9092"
            };
            _producer = new ProducerBuilder<Null, string>(config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
        }

        public async Task SendMessageToKafka(TransferEntity transfer)
        {
            _logger.LogInformation("Sending the message to kafka topic");
            try
            {
                var transferInformation = JsonConvert.SerializeObject(transfer);
                var message = await _producer.ProduceAsync("fund-transfer", new Message<Null, string>() { Value = transferInformation });
                _logger.LogInformation($"Message sent: {message.Message.Value} | Offset: {message.Offset}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | Message: {ex.Message}");
                throw;
            }
        }
    }
}
