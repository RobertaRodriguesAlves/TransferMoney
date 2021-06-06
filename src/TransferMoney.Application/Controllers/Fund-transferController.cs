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

        [HttpGet]
        [Route("{transactionId}")]
        public async Task<ActionResult> Get(string transactionId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var response = await _fundTransferService.Get(transactionId);
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
