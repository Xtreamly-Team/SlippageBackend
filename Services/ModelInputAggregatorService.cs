using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SlippageBackend.Models;

namespace SlippageBackend.Services;

public class ModelInputAggregatorService (IMongoClient _client)
{
    public async Task<double> GetLiquidity(string poolAddress)
    {
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<LpReport>("LPInfo")
            .AsQueryable()
            .Where(lp_info => lp_info.PoolAddress == poolAddress
            )
            .OrderByDescending(lp_info => lp_info.Timestamp)
            .FirstOrDefault();
        if (lpReport == null)
        {
            throw new Exception("No LP info found");
        }
        return    double.Parse(lpReport.Liquidity) ;

    }
    
    public async Task<double> GetlpTvlToken0(string poolAddress)
    {
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<LpReport>("LPInfo")
            .AsQueryable()
            .Where(lp_info => lp_info.PoolAddress == poolAddress
            )
            .OrderByDescending(lp_info => lp_info.Timestamp)
            .FirstOrDefault();
        if (lpReport == null)
        {
            throw new Exception("No LP info found");
        }
        return    (lpReport.ValueLockedToken0) ;

    }
    
    public async Task<double> GetlpTvlToken1(string poolAddress)
    {
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<LpReport>("LPInfo")
            .AsQueryable()
            .Where(lp_info => lp_info.PoolAddress == poolAddress
            )
            .OrderByDescending(lp_info => lp_info.Timestamp)
            .FirstOrDefault();
        if (lpReport == null)
        {
            throw new Exception("No LP info found");
        }
        return    (lpReport.ValueLockedToken1) ;

    }
    
    public async Task<double> GetlpTvlUSD(string poolAddress)
    {
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<LpReport>("LPInfo")
            .AsQueryable()
            .Where(lp_info => lp_info.PoolAddress == poolAddress
            )
            .OrderByDescending(lp_info => lp_info.Timestamp)
            .FirstOrDefault();
        if (lpReport == null)
        {
            throw new Exception("No LP info found");
        }
        return    (lpReport.TotalValueLockedInTermOfToken1)  * 1;

    }
    
    public async Task<double> GetVolumeUSD(string poolAddress)
    {
        var lpReport = await _client.GetDatabase("xtreamly").GetCollection<BsonDocument>("UNISWAP_REALTIME")
            .AsQueryable()
            .SumAsync(doc => double.Parse(doc["Event"]["amount1Pure"].ToString()));
        return    (lpReport)  * 1;

    }
    
    
}