using System.Collections.Immutable;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
        return lpReport["ValueLockedToken0"].AsDouble;
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
        return lpReport["ValueLockedToken1"].AsDouble;
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
        return lpReport["totalValueLockedInTermOfToken1"].AsDouble;
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
   

    private async Task<double> GetCurrentVolume(string poolAddress)
    {
        var todayStart = new DateTimeOffset(DateTime.UtcNow.Date).ToUnixTimeMilliseconds().ToString();

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Regex(e => e["Log"]["Address"].AsString,
                new BsonRegularExpression($".*{poolAddress}.*", "i")),
            Builders<BsonDocument>.Filter.Gte("Event.Timestamp", todayStart)
        );
        var qpReport = _client
            .GetDatabase("xtreamly")
            .GetCollection<BsonDocument>("UNISWAP_REALTIME")
            .Find(filter)
            .ToList()
            .Select(doc => Math.Abs(double.Parse(doc["Event"]["amount1Pure"].AsString)))
            .Sum();

        return qpReport;
    }


    public async Task<OHLCVResult> GetOHLCVAsync()
    {
        // it is 14 seconds behind the current utc
        var start_time =  new DateTimeOffset( DateTime.UtcNow.AddSeconds(-14)).ToUnixTimeMilliseconds();

        var collection = _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("CEX_Raw_Trade");
        
        var timestampFilter = Builders<BsonDocument>.Filter.Gt("timestamp", start_time);
        var symbolFilter = Builders<BsonDocument>.Filter.Eq("symbol", "ETH-USDT");
        var combinedFilter = Builders<BsonDocument>.Filter.And(timestampFilter, symbolFilter);

        var trades = await collection.Find(combinedFilter).ToListAsync();
        //var trades =  await  collection.AsQueryable().Where(doc => doc["timestamp"].AsInt64 > start_time).ToListAsync();
        decimal? open_price = null;
        var high_price = decimal.MinValue;
        var low_price = decimal.MaxValue;
        decimal close_price = 0;
        decimal total_volume = 0;

        foreach (var trade in trades)
        {
            var price =(decimal) trade["price"].ToDouble();
            var amount = (decimal) trade["amount"].ToDouble();

            open_price ??= price;

            if (price > high_price) high_price = price;

            if (price < low_price) low_price = price;

            close_price = price;
            total_volume += amount;
        }
        // log these to console
        Console.WriteLine($"Open: {open_price}, High: {high_price}, Low: {low_price}, Close: {close_price}, Volume: {total_volume}");
        return new OHLCVResult(open_price.Value, high_price, low_price, close_price, total_volume);
    }
}