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
            
            var cexData = await aggregatorService.GetOHLCVAsync();
            var quotedPrice = (decimal)await aggregatorService.GetQuotedPrice(poolAddress);
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
                //LpVolumeUsd = (decimal) await aggregatorService.GetVolumeUSD(poolAddress),
                Close14s =cexData.Close,
                High14s = cexData.High,
                Low14s = cexData.Low,
                Open14S = cexData.Open,
                Volume14s = cexData.Volume
            };
            var result = await _communicationService.ExecuteInference(modelInput);
            result!.Slippage = ((( result.ExecutionPrice) / quotedPrice ) -1 ) * 100;
            return Ok(result);
        }
    }
}
