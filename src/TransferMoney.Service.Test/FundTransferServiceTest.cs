using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;
using TransferMoney.Domain.Interfaces.Repository;
using TransferMoney.Service.Services;
using Xunit;

namespace TransferMoney.Service.Test
{
    public class FundTransferServiceTest
    {
        private IFundTransferService _service;
        private Mock<ILogger<FundTransferService>> _logger;
        private Mock<ITransferMoneyRepository> _repositoryMock;
        private Mock<ITransferMoneyProducerKafka> _transferMoneyProducerKafka;

        public FundTransferServiceTest()
        {
            _logger = new Mock<ILogger<FundTransferService>>();
            _repositoryMock = new Mock<ITransferMoneyRepository>();
            _transferMoneyProducerKafka = new Mock<ITransferMoneyProducerKafka>();
            _service = new FundTransferService(_transferMoneyProducerKafka.Object, _repositoryMock.Object, _logger.Object);
        }

        [Fact]
        public async Task GivenAValidTransaction_ItShouldCallTheGetStatusMethodOnce()
        {
            await _service.GetTransactionStatus(Guid.NewGuid().ToString());
            _repositoryMock.Verify(repo => repo.GetStatusAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GivenAnInvalidTransaction_ItShouldReturnNotFound()
        {
            _repositoryMock.Setup(repo => repo.GetStatusAsync(It.IsAny<string>())).ReturnsAsync((TransferEntity)null);
            var result = await _service.GetTransactionStatus(Guid.NewGuid().ToString());
            Assert.Equal("Not Found", result);
        }

        [Fact]
        public async Task GivenAnInQueueTransaction_ItShouldReturnInQueue()
        {
            _repositoryMock.Setup(repo => repo.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(new TransferEntity {Status = null});
            var result = await _service.GetTransactionStatus(Guid.NewGuid().ToString());
            Assert.Equal("In Queue", result);
        }

        [Fact]
        public async Task GivenAConfirmedTransaction_ItShouldReturnConfirmed()
        {
            _repositoryMock.Setup(repo => repo.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(new TransferEntity { Status = "Confirmed", Message = null });
            var result = await _service.GetTransactionStatus(Guid.NewGuid().ToString());
            Assert.Equal("Confirmed", result);
        }

        [Fact]
        public async Task GivenAnErrorTransaction_ItShouldReturnError()
        {
            _repositoryMock.Setup(repo => repo.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(new TransferEntity { Status = "Error", Message = "Any" });
            var result = await _service.GetTransactionStatus(Guid.NewGuid().ToString());
            Assert.IsType<FullResponseDto>(result);
        }

        [Fact]
        public async Task ShouldCallTheInsertAsyncMethodAndSendMessageToKafkaMethod_WhenATransferDtoIsInformed()
        {
            await _service.Post(new TransferDto());
            _repositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<TransferEntity>()), Times.Once());
            _transferMoneyProducerKafka.Verify(kafka => kafka.SendMessageToKafka(It.IsAny<TransferEntity>()), Times.Once());
        }

    }
}
