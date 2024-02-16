using System.Text.Json.Serialization;

namespace SlippageBackend.Models
{
    public partial class ModelOutput
    {
        [JsonPropertyName("execution_price")]
        public long ExecutionPrice { get; set; }

        [JsonPropertyName("slippage_amount")]
        public long SlippageAmount { get; set; }

        [JsonPropertyName("slippage_percentage")]
        public long SlippagePercentage { get; set; }
    }
}