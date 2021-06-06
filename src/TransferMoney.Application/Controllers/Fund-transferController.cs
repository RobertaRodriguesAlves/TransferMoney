using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Application.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class fund_transferController : ControllerBase
    {
        private readonly ILogger<fund_transferController> _logger;
        private readonly IFundTransferService _fundTransferService;

        public fund_transferController(ILogger<fund_transferController> logger,
                                        IFundTransferService fundTransferService)
        {
            _logger = logger;
            _fundTransferService = fundTransferService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] TransferDto transfer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var transactionResult = new TransferDtoResult
                {
                    TransactionId = _fundTransferService.Post(transfer)
                };
                return Ok(transactionResult);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            } 
        }
    }
}
