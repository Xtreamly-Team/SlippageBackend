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

        [JsonPropertyName("lp_Volume50")]
        public decimal LpVolumeUsd { get; set; }

        [JsonPropertyName("open14s")]
        public decimal Open14S { get; set; }
        
        [JsonPropertyName("high14s")]
        public decimal High14s { get; set; }
        
        [JsonPropertyName("low14s")]
        public decimal Low14s { get; set; }
        
        [JsonPropertyName("close14s")]
        public decimal Close14s { get; set; }
        
        [JsonPropertyName("volume14s")]
        public decimal Volume14s { get; set; }
        
        [JsonPropertyName("ma50")]
        public decimal Ma50 { get; set; }
        
        [JsonPropertyName("ma100")]
        public decimal Ma100 { get; set; }
        
        
        
    }
}