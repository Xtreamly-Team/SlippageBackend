using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlippageBackend.Models;
using SlippageBackend.Services;

namespace SlippageBackend.Controllers.v1.Slippage
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize (AuthenticationSchemes = BearerTokenDefaults.AuthenticationScheme)]
    public class Slippage ( ILogger<Slippage> _logger , ModelInputAggregatorService aggregatorService , ModelCommunicationService _communicationService) : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult> CalculateSlippage([FromQuery] decimal amountIn, [FromQuery] long gasPrice , [FromQuery] bool isBuy , [FromQuery] string poolAddress, [FromQuery] int feeTier)
        {
            var modelInput = new ModelInput()
            {
                AmountIn = amountIn,
                GasPrice = gasPrice,
                IsBuy = isBuy,
                LpFeeTier = feeTier,
                QuotePrice = (decimal)await aggregatorService.GetQuotedPrice(poolAddress),
                LpLiquidity = (decimal)await aggregatorService.GetLiquidity(poolAddress),
                LpTvlToken0 = (decimal)await aggregatorService.GetlpTvlToken0(poolAddress),
                LpTvlToken1 = (decimal)await aggregatorService.GetlpTvlToken1(poolAddress),
                LpTvlUsd = (decimal)await aggregatorService.GetlpTvlUSD(poolAddress),
                LpVolumeUsd = (decimal)await aggregatorService.GetVolumeUSD(poolAddress),
                LpVolumeUsdChange = (decimal)await aggregatorService.GetVolumeUSDChanged(poolAddress),  
                LpVolumeUsdWeek = 0,  
            };
            //var result = await _communicationService.ExecuteInference(modelInput);
            return Ok(new ModelOutput()
            {
                Slippage = -1.5m,
                ExecutionPrice = 2450.2m
            });
        }
    }
}
