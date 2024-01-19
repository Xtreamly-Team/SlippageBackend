using System.Text.Json.Serialization;

namespace SlippageBackend.Models
{
    public partial class ModelOutput
    {
        [JsonPropertyName("execution_price")]
        public decimal ExecutionPrice { get; set; }

        [JsonPropertyName("slippage")]
        public decimal Slippage { get; set; }
    }
}