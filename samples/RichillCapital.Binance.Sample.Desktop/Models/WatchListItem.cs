namespace RichillCapital.Binance.Sample.Desktop.Models;

public sealed record WatchListItem 
{
    public required string Symbol { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
}
