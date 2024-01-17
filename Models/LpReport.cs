using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SlippageBackend.Models;

public class LpReport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Liquidity { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public double Token0Price { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public double Token1Price { get; set; }

    public string Token0Balance { get; set; }

    public string Token1Balance { get; set; }

    public string Token0Address { get; set; }

    public string Token1Address { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public double TotalValueLockedInTermOfToken1 { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public double ValueLockedToken1 { get; set; }

    [BsonRepresentation(BsonType.Double)]
    public double ValueLockedToken0 { get; set; }

    public int FeeProtocol { get; set; }

    public int Tick { get; set; }

    public string SqrtPriceX96 { get; set; }

    public string BlockNumber { get; set; }

    public string PoolAddress { get; set; }

    [BsonRepresentation(BsonType.Int64)]
    public long Timestamp { get; set; }
}