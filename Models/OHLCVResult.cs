namespace SlippageBackend.Models;
public class OHLCVResult
{
    public double? Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double? Close { get; set; }
    public double Volume { get; set; }

    public OHLCVResult(double? open, double high, double low, double? close, double volume)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
}
