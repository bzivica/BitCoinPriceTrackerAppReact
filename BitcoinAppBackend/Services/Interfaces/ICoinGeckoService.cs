namespace BitcoinAppBackend.Services.Interfaces;

public interface ICoinGeckoService
{
    Task<decimal> GetBitcoinPriceInEURAsync();
}
