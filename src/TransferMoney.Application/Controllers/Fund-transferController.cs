using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
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
        public async Task<ActionResult> Post([FromBody] TransferEntity transfer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var result = _fundTransferService.Post(transfer);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return Ok();
        }
    }
}
