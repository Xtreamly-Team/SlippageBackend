using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SlippageBackend.Models;

public class LpReport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }
    
    [BsonElement("liquidity")]
    public string Liquidity { get; set; }

    [BsonElement("token0Price")]
    [BsonRepresentation(BsonType.Double)]
    public double Token0Price { get; set; }
    
    
    [BsonElement("token1Price")]
    [BsonRepresentation(BsonType.Double)]
    public double Token1Price { get; set; }

    
    [BsonElement("token0Balance")]
    public string Token0Balance { get; set; }

    [BsonElement("token1Balance")]
    public string Token1Balance { get; set; }

    [BsonElement("token0Address")]
    public string Token0Address { get; set; }

    [BsonElement("token1Address")]
    public string Token1Address { get; set; }

    [BsonRepresentation(BsonType.Double)]
    [BsonElement("totalValueLockedInTermOfToken1")]
    public double TotalValueLockedInTermOfToken1 { get; set; }

    [BsonElement("ValueLockedToken1")]
    [BsonRepresentation(BsonType.Double)]
    public double ValueLockedToken1 { get; set; }

    [BsonElement("ValueLockedToken0")]
    [BsonRepresentation(BsonType.Double)]
    public double ValueLockedToken0 { get; set; }

    [BsonElement("feeProtocol")]
    public int FeeProtocol { get; set; }
    
    
    
    [BsonElement("tick")] 
    public int Tick { get; set; }
    
    
[BsonElement("sqrtPriceX96")] 
    public string SqrtPriceX96 { get; set; }

    [BsonElement("blockNumber")] 
    public string BlockNumber { get; set; }

    [BsonElement("poolAddress")] 
    public string PoolAddress { get; set; }

     [BsonElement("timestamp")] 
    [BsonRepresentation(BsonType.Int64)]
    public long Timestamp { get; set; }
}