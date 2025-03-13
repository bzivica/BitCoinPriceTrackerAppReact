namespace BitcoinAppBackend.Services.Interfaces;
public interface IBitcoinPriceCalculatorService
{
    Task<(decimal BitcoinPriceEUR, decimal BitcoinPriceCZK)> GetBitcoinPriceAsync();
}
