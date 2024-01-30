using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SlippageBackend.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SlippageBackend.Services;

public class ModelInputAggregatorService(IMongoClient _client, IHttpClientFactory _clientFactory)
{
    public async Task<double> GetLiquidity(string poolAddress)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("poolAddress", poolAddress);
        var sort = Builders<BsonDocument>.Sort.Descending("timestamp");
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("LPInfo")
            .Find(filter)
            .Sort(sort)
            .FirstOrDefault();
        if (lpReport == null) throw new Exception("No LP info found");
        return double.Parse(lpReport["liquidity"].AsString);
    }

    public async Task<double> GetlpTvlToken0(string poolAddress)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("poolAddress", poolAddress);
        var sort = Builders<BsonDocument>.Sort.Descending("timestamp");
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("LPInfo")
            .Find(filter)
            .Sort(sort)
            .FirstOrDefault();
        if (lpReport == null) throw new Exception("No LP info found");
        return (lpReport["ValueLockedToken0"].AsDouble);
    }

    public async Task<double> GetlpTvlToken1(string poolAddress)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("poolAddress", poolAddress);
        var sort = Builders<BsonDocument>.Sort.Descending("timestamp");

        var lpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("LPInfo")
            .Find(filter)
            .Sort(sort)
            .FirstOrDefault();
        if (lpReport == null) throw new Exception("No LP info found");
        return (lpReport["ValueLockedToken1"].AsDouble);
    }

    public async Task<double> GetlpTvlUSD(string poolAddress)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("poolAddress", poolAddress);
        var sort = Builders<BsonDocument>.Sort.Descending("timestamp");
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("LPInfo")
            .Find(filter)
            .Sort(sort)
            .FirstOrDefault();
        if (lpReport == null) throw new Exception("No LP info found");
        return (lpReport["totalValueLockedInTermOfToken1"].AsDouble);
    }


    public async Task<double> GetQuotedPrice(string poolAddress)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("poolAddress", poolAddress);
        var sort = Builders<BsonDocument>.Sort.Descending("blockNumber");
        var qpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("QuotedPrice")
            .Find(filter)
            .Sort(sort)
            .FirstOrDefault();
        if (qpReport == null) throw new Exception("No LP info found");
        return double.Parse(qpReport["quotedPrice"].AsString);
    }

    public async Task<double> GetVolumeUSD(string poolAddress)
    {
        var currentVolume = await GetCurrentVolume(poolAddress);
        return currentVolume;
    }

    public async Task<double> GetVolumeUSDChanged(string poolAddress)
    {
        var currentVolume = await GetCurrentVolume(poolAddress) / 10e5;
        var yesterdayVolume = await GetVolumeUSDForDaysAgo(poolAddress, 1);
        var twoDaysAgoVolume = await GetVolumeUSDForDaysAgo(poolAddress, 2);
        var part1 = currentVolume - yesterdayVolume;
        var part2 = yesterdayVolume - twoDaysAgoVolume;
        return ((part1 - part2) / part2) * 100;
    }

    private async Task<double> GetVolumeUSDForDaysAgo(string poolAddress, int daysAgo)
    {
        poolAddress = poolAddress.ToLower();
        var httpClient = _clientFactory.CreateClient();
        var time = new DateTimeOffset(DateTime.UtcNow - TimeSpan.FromDays(daysAgo - 1)).ToUnixTimeSeconds();
        var request = new
        {
            query = """
                {
                  pool(id: "POOL_ADDRESS") {
                    id
                    poolDayData(
                      orderBy: date
                      orderDirection: desc
                      first: 1
                      where: {date_lt: date_lt_value}
                    ) {
                      volumeUSD
                      date
                    }
                  }
                }
                """.Replace("POOL_ADDRESS", poolAddress)
                .Replace("date_lt_value", time.ToString())
        };

        var jsonRequest = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var responseResult = await httpClient.PostAsync(Consts.Consts.UNISWAP_V3_THE_GRAPH, content);
        responseResult.EnsureSuccessStatusCode();
        var response = await responseResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TheGraphResult>(response);
        return double.Parse(result?.Data.Pool.PoolDayData[0].VolumeUsd);
    }

    private async Task<double> GetCurrentVolume(string poolAddress)
    {
        poolAddress = poolAddress.ToLower();
        var todayStart = new DateTimeOffset(DateTime.UtcNow.Date).ToUnixTimeMilliseconds().ToString() ;

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("Log.Address", poolAddress),
            Builders<BsonDocument>.Filter.Gte("Event.Timestamp", todayStart)

        );
        var qpReport = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("UNISWAP_REALTIME")
            .Find(filter)
            .ToList()
            .Select(doc => Math.Abs( double.Parse(doc["Event"]["amount1Pure"].AsString)))
            .Sum();

        return  qpReport;

    }
}