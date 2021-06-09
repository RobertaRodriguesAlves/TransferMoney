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
        public async Task WhenTheModelStateIsInvalid_ThePostMethodShouldRespondWithBadRequest()
        {
            var transfer = new TransferDto
            {
                AccountDestination = "54325432",
                Value = 434432
            };

            _transferController.ModelState.AddModelError("AccountOrigin", "Account origin is required");

            var transferControllerPostMethodResult = await _transferController.Post(transfer);
            Assert.True(transferControllerPostMethodResult is BadRequestResult);
        }

        [Fact]
        public async Task WhenAValidObjectIsPassed_ThePostMethodShouldResponseWithOk()
        {
            _serviceMock.Setup(config => config.Post(It.IsAny<TransferDto>())).ReturnsAsync(Guid.NewGuid().ToString());
            var transfer = new TransferDto
            {
                AccountDestination = "54325432",
                AccountOrigin = "56757432",
                Value = 434432
            };

            var transferControllerPostMethodResult = await _transferController.Post(transfer);

            Assert.IsType<OkObjectResult>(transferControllerPostMethodResult);
        }

        [Fact]
        public async Task WhenAValidTransactionIdIsPassed_TheGetMethodShouldRespondWithOK()
        {
            _serviceMock.Setup(config => config.GetTransactionStatus(It.IsAny<string>())).ReturnsAsync(new FullResponseDto
            {
                Status = "Error",
                Message = "AccountOrigin doesn't exist"
            });

            var transactionId = Guid.NewGuid().ToString();
            var transferControllerGetMethodResult = await _transferController.Get(transactionId);

            Assert.True(transferControllerGetMethodResult is OkObjectResult);
        }

        [Fact]
        public async Task WhenTheTransactionIdIsInvalid_TheGetMethodShouldRespondWithOkResult()
        {
            _serviceMock.Setup(config => config.GetTransactionStatus(It.IsAny<string>())).ReturnsAsync(new ResponseDto
            {
                Status = "Not Found"
            });
            var transactionId = "Banana";

            var transferControllerPostMethodResult = await _transferController.Get(transactionId);
            Assert.True(transferControllerPostMethodResult is OkObjectResult);
        }
    }
}
