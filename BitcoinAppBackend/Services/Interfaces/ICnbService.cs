namespace BitcoinAppBackend.Services.Interfaces;

// ICnbService.cs
public interface ICnbService
{
    Task<decimal> GetEurToCzkRateAsync();
}
