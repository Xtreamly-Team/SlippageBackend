using System.Text.Json.Serialization;

namespace SlippageBackend.Models
{
    public partial class ModelOutput
    {
        [JsonPropertyName("execution_price")]
        public decimal ExecutionPrice { get; set; }

        [JsonPropertyName("slippage_amount")]
        public decimal SlippageAmount { get; set; }

        [JsonPropertyName("slippage_percentage")]
        public decimal SlippagePercentage { get; set; }
        
        public string Debug { get; set; } 
    }
}