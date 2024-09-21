namespace RichillCapital.Binance.Spot.Contracts;

internal static class BinanceSpotApiRoutes
{
    internal static class General
    {
        internal const string Ping = "api/v3/ping";
        internal const string ServerTime = "api/v3/time";
        internal const string ExchangeInfo = "api/v3/exchangeInfo";
    }

    internal static class Trading
    {
        internal const string NewOrder = "api/v3/order";
        internal const string TestNewOrder = "api/v3/order/test";
    }
}