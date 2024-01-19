using System.Text.Json.Serialization;

namespace SlippageBackend.Models
{
    public partial class TheGraphResult
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("pool")]
        public Pool Pool { get; set; }
    }

    public partial class Pool
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("poolDayData")]
        public PoolDayDatum[] PoolDayData { get; set; }
    }

    public partial class PoolDayDatum
    {
        [JsonPropertyName("volumeUSD")]
        public string? VolumeUsd { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }
    }
}
