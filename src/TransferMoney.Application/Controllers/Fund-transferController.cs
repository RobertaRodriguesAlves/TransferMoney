using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Application.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Fund_transferController : ControllerBase
    {
        private readonly ILogger<Fund_transferController> _logger;
        private readonly IFundTransferService _fundTransferService;

        public Fund_transferController(ILogger<Fund_transferController> logger,
                                        IFundTransferService fundTransferService)
        {
            _logger = logger;
            _fundTransferService = fundTransferService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TransferDto transfer)
        {
            try
            {
                _logger.LogInformation("Validating the model state of the transfer");
                if (!ModelState.IsValid)
                    return BadRequest();

                _logger.LogInformation($"Starting the transfer requisition between: {transfer.AccountOrigin} and {transfer.AccountDestination}");

                var transactionResult = new TransferDtoResult
                {
                    TransactionId = await _fundTransferService.Post(transfer)
                };

                _logger.LogInformation("Finishing the transfer requisition");
                return Ok(transactionResult);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            } 
        }

        [HttpGet]
        [Route("{transactionId}")]
        public async Task<ActionResult> Get(string transactionId)
        {
            try
            {
                _logger.LogInformation("Validating the model state of the transactionId");
                if (!ModelState.IsValid)
                    return BadRequest();

                _logger.LogInformation($"Starting the search for the {transactionId} in the database");
                var response = await _fundTransferService.GetTransactionStatus(transactionId);
                _logger.LogInformation("Finishing the search");
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
