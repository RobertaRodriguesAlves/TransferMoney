using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TransferMoney.Application.Controllers;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;
using Xunit;

namespace TransferMoney.Application.Test
{
    public class Fund_transferControllerTest
    {
        private Fund_transferController _transferController;
        private Mock<ILogger<Fund_transferController>> _logger;
        private Mock<IFundTransferService> _serviceMock;
        public Fund_transferControllerTest()
        {
            _serviceMock = new Mock<IFundTransferService>();
            _logger = new Mock<ILogger<Fund_transferController>>();
            _transferController = new Fund_transferController(_logger.Object, _serviceMock.Object); 
        }

        [Fact]
        public void WhenTheModelStateIsInvalid_ThePostMethodShouldRespondWithBadRequest()
        {
            var transfer = new TransferDto
            {
                AccountDestination = "54325432",
                Value = 434432
            };

            _transferController.ModelState.AddModelError("AccountOrigin", "Account origin is required");

            var transferControllerPostMethodResult = _transferController.Post(transfer);
            Assert.True(transferControllerPostMethodResult is BadRequestResult);
        }

        [Fact]
        public void WhenAValidObjectIsPassed_ThePostMethodShouldResponseWithOk()
        {
            _serviceMock.Setup(config => config.Post(It.IsAny<TransferDto>())).Returns(Guid.NewGuid().ToString());
            var transfer = new TransferDto
            {
                AccountDestination = "54325432",
                AccountOrigin = "56757432",
                Value = 434432
            };

            var transferControllerPostMethodResult = _transferController.Post(transfer);

            Assert.IsType<OkObjectResult>(transferControllerPostMethodResult);
        }

        [Fact]
        public async Task WhenAValidTransactionIdIsPassed_TheGetMethodShouldRespondWithOK()
        {
            _serviceMock.Setup(config => config.Get(It.IsAny<string>())).ReturnsAsync(new FullResponseDto
            {
                Status = "Error",
                Message = "AccountOrigin doesn't exist"
            });

            var transactionId = Guid.NewGuid().ToString();
            var transferControllerGetMethodResult = await _transferController.Get(transactionId);

            Assert.True(transferControllerGetMethodResult is OkObjectResult);
        }

        [Fact]
        public void WhenTheModelStateIsInvalid_TheGetMethodShouldRespondWithBadRequest()
        {
            //_transferController.ModelState.AddModelError("TransactionId", "Account origin is required");

            //var transferControllerPostMethodResult = _transferController.Post(transfer);
            //Assert.True(transferControllerPostMethodResult is BadRequestResult);
        }

    }
}
