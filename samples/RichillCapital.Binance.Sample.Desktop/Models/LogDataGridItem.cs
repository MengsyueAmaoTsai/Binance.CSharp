namespace RichillCapital.Binance.Sample.Desktop.Models;

public sealed record LogDataGridItem
{
    public required DateTimeOffset Time { get; init; }
    public required string Level { get; init; }
    public required string Message { get; init; }
}
