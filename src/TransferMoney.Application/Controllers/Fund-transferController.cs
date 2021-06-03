using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Application.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class fund_transferController : ControllerBase
    {
        private readonly ILogger<fund_transferController> _logger;

        public fund_transferController(ILogger<fund_transferController> logger) => _logger = logger;

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TransferDto transfer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();



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
