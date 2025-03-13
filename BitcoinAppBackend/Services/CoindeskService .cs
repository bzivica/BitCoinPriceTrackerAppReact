using System;
using System.Net.Http;
using System.Threading.Tasks;
using BitcoinAppBackend.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BitcoinAppBackend.Services;

public class CoindeskService : ICoindeskService
{
    private readonly string _coindeskApiUrl;
    private readonly HttpClient _httpClient;

    // Constructor that takes IConfiguration to load the API URL from appsettings.json
    public CoindeskService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _coindeskApiUrl = configuration["AppSettings:CoindeskApiUrl"] ?? throw new ArgumentNullException("CoindeskApiUrl", "API URL must be configured.");
    }

    // Asynchronous method to get the Bitcoin price in EUR from Coindesk API
    public async Task<decimal> GetBitcoinPriceInEURAsync()
    {
        try
        {
            // Send a GET request to the Coindesk API and get the response as a string
            var response = await _httpClient.GetStringAsync(_coindeskApiUrl);
            var data = JsonConvert.DeserializeObject<dynamic>(response);
            decimal priceInEUR = data.bpi.EUR.rate_float;
            return priceInEUR;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while fetching the Bitcoin price in EUR from Coindesk: {ex.Message}", ex);
        }
    }
}
