using BitcoinAppBackend.Services.Interfaces;
using System;
using System.Threading.Tasks;

public class BitcoinPriceCalculatorService : IBitcoinPriceCalculatorService
{
    private readonly ICoindeskService _coinDeskService;
    private readonly ICnbService _cnbService;
    private readonly ICoinGeckoService _coinGeckoService;

    public BitcoinPriceCalculatorService(ICoindeskService coindeskService, ICnbService cnbService, ICoinGeckoService coinGeckoService)
    {
        _coinDeskService = coindeskService;
        _coinGeckoService = coinGeckoService;
        _cnbService = cnbService;
    }

    public async Task<(decimal BitcoinPriceEUR, decimal BitcoinPriceCZK)> GetBitcoinPriceAsync()
    {
        decimal bitcoinPriceEUR = 0;
        decimal bitcoinPriceCZK = 0;
        decimal eurToCzkRate = await _cnbService.GetEurToCzkRateAsync();

        try
        {
            // Attempt to fetch Bitcoin price from Coindesk API
            bitcoinPriceEUR = await _coinDeskService.GetBitcoinPriceInEURAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching from Coindesk: {ex.Message}");
            // If Coindesk fails, attempt to fetch Bitcoin price from CoinGecko API
            try
            {
                bitcoinPriceEUR = await _coinGeckoService.GetBitcoinPriceInEURAsync();
            }
            catch (Exception innerEx)
            {
                Console.WriteLine($"Error fetching from CoinGecko: {innerEx.Message}");
                // Handle or rethrow the exception as needed
                throw new Exception("Failed to fetch Bitcoin price from both Coindesk and CoinGecko.");
            }
        }

        // Calculate Bitcoin price in CZK if the EUR price was successfully fetched
        if (bitcoinPriceEUR > 0)
        {
            bitcoinPriceCZK = bitcoinPriceEUR * eurToCzkRate;
        }
        else
        {
            Console.WriteLine("Failed to retrieve Bitcoin price in EUR.");
        }

        return (bitcoinPriceEUR, bitcoinPriceCZK);
    }
}
