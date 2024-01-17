using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlippageBackend.Services;

namespace SlippageBackend.Controllers.v1.Slippage
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize (AuthenticationSchemes = BearerTokenDefaults.AuthenticationScheme)]
    public class Slippage ( ILogger<Slippage> _logger , ModelInputAggregatorService aggregatorService) : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult> CalculateSlippage([FromQuery] decimal amountIn, [FromQuery] decimal gasPrice , [FromQuery] bool isBuy , [FromQuery] string poolAddress)
        {
            var x = await aggregatorService.GetLiquidity(poolAddress);
            return Ok("Hello World!");
        }
    }
}
