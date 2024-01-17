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
    
    p
    
}