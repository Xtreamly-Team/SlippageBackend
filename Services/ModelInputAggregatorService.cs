using MongoDB.Driver;
using SlippageBackend.Models;

namespace SlippageBackend.Services;

public class ModelInputAggregatorService (IMongoClient _client)
{
    public async Task<double> GetLiquidity()
    {
        var lpReport = _client.GetDatabase("xtreamly").GetCollection<LpReport>("LPInfo")
            .AsQueryable()
            .OrderByDescending(lp_info => lp_info.Timestamp)
            .FirstOrDefault();
        if (lpReport == null)
        {
            throw new Exception("No LP info found");
        }
        return    double.Parse(lpReport.Liquidity) ;

    }
    
}