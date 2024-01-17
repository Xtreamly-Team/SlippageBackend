using System.Text.Json.Serialization;

namespace SlippageBackend.Models
{
    public partial class ModelInput
    {
        [JsonPropertyName("amount_in")]
        public decimal AmountIn { get; set; }

        [JsonPropertyName("quote_price")]
        public decimal QuotePrice { get; set; }

        [JsonPropertyName("gas_price")]
        public long GasPrice { get; set; }

        [JsonPropertyName("is_buy")]
        public bool IsBuy { get; set; }

        [JsonPropertyName("lp_feeTier")]
        public long LpFeeTier { get; set; }

        [JsonPropertyName("lp_liquidity")]
        public decimal LpLiquidity { get; set; }

        [JsonPropertyName("lp_tvlToken0")]
        public decimal LpTvlToken0 { get; set; }

        [JsonPropertyName("lp_tvlToken1")]
        public decimal LpTvlToken1 { get; set; }

        [JsonPropertyName("lp_tvlUSD")]
        public decimal LpTvlUsd { get; set; }

        [JsonPropertyName("lp_volumeUSD")]
        public decimal LpVolumeUsd { get; set; }

        [JsonPropertyName("lp_volumeUSDChange")]
        public decimal LpVolumeUsdChange { get; set; }

        [JsonPropertyName("lp_volumeUSDWeek")]
        public decimal LpVolumeUsdWeek { get; set; }
    }
}