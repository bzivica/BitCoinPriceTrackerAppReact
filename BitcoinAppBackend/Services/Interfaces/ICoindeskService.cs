namespace BitcoinAppBackend.Services.Interfaces;

public interface ICoindeskService
{
    Task<decimal> GetBitcoinPriceInEURAsync();
}

