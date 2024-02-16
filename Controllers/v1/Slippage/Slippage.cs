using Microsoft.AspNetCore.Mvc;
using SlippageBackend.Models;
using SlippageBackend.Services;

namespace SlippageBackend.Controllers.v1.Slippage;

[Route("api/v1/[controller]")]
[ApiController]
//[Authorize (AuthenticationSchemes = BearerTokenDefaults.AuthenticationScheme)]
public class Slippage(
    ILogger<Slippage> _logger,
    ModelInputAggregatorService aggregatorService,
    ModelCommunicationService _communicationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> CalculateSlippage([FromQuery] decimal amountIn, [FromQuery] long gasPrice,
        [FromQuery] bool isBuy, [FromQuery] string poolAddress, [FromQuery] int feeTier,  [FromQuery] string symbol)
    {
        //"ETH-USDT", "ETH-USDC"
        var cexData = await aggregatorService.GetOHLCVAsync(symbol);
        var quotedPrice = (decimal)await aggregatorService.GetQuotedPrice(poolAddress);
        var modelInput = new ModelInput
        {
            AmountIn = amountIn,
            GasPrice = gasPrice,
            IsBuy = isBuy,
            LpFeeTier = feeTier,
            QuotePrice = quotedPrice,
            LpLiquidity = (decimal)await aggregatorService.GetLiquidity(poolAddress),
            LpTvlToken0 = (decimal)await aggregatorService.GetlpTvlToken0(poolAddress),
            LpTvlToken1 = (decimal)await aggregatorService.GetlpTvlToken1(poolAddress),
            LpTvlUsd = (decimal)await aggregatorService.GetlpTvlUSD(poolAddress),
            LpVolumeUsd = (decimal)await aggregatorService.GetCurrentVolume(poolAddress),
            Close14s = cexData.Close,
            High14s = cexData.High,
            Low14s = cexData.Low,
            Open14S = cexData.Open,
            Volume14s = cexData.Volume,
            Ma50 = (decimal)await aggregatorService.GetMa50(symbol),
            Ma100 = (decimal) await aggregatorService.GetMa100(symbol)
        };
        var result = await _communicationService.ExecuteInference(modelInput, symbol);
        _logger.LogInformation( System.Text.Json.JsonSerializer.Serialize(modelInput!));
        return Ok(result);
    }
}