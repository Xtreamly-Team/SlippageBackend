namespace SlippageBackend.Models;
public class OHLCVResult
{
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }

    public OHLCVResult(decimal open, decimal high, decimal low, decimal close, decimal volume)
    {
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
}
