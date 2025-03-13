using System.Net.Http;
using System.Threading.Tasks;
using BitcoinAppBackend.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitcoinAppBackend.Services;

/// <summary>
/// Service for fetching EUR to CZK exchange rate from the Czech National Bank (CNB) API.
/// Implements ICnbService to provide an abstraction for exchange rate retrieval.
/// </summary>
public class CnbService : ICnbService
{
    private readonly string _baseApiUrl;
    private readonly HttpClient _httpClient;
    private const string CnbApiUrlKey = "AppSettings:CnbApiUrl";

    // Constructor that injects the HttpClient and IConfiguration dependencies
    public CnbService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseApiUrl = configuration[CnbApiUrlKey] ?? throw new ArgumentNullException("CnbApiUrl", "Base API URL must be configured.");
    }

    // Asynchronous method to fetch EUR to CZK exchange rate
    public async Task<decimal> GetEurToCzkRateAsync()
    {
        // Build the full API URL by appending the current date to the base URL
        string cnbApiUrl = $"{_baseApiUrl}{DateTime.Now:yyyy-MM-dd}";

        try
        {
            // Send a GET request to the CNB API and get the response as a string
            var response = await _httpClient.GetStringAsync(cnbApiUrl);

            // Parse the JSON response from the CNB API
            JObject json = JObject.Parse(response);

            // Access the "rates" object in the response
            var rates = json["rates"];
            if (rates == null || rates.Count() == 0)
            {
                throw new Exception("Rates not found or empty in the response.");
            }

            // Search for the EUR currency in the "rates" object
            var eurRate = rates.FirstOrDefault(rate => rate["currencyCode"]?.ToString() == "EUR");
            if (eurRate == null)
            {
                throw new Exception("EUR rate not found in the response.");
            }

            // Retrieve the "rate" value for EUR
            var rate = eurRate["rate"];
            if (rate == null)
            {
                throw new Exception("Rate value not found in the response.");
            }

            // Return the rate value as a decimal
            return rate.Value<decimal>();
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., network errors, API issues, etc.)
            throw new Exception($"An error occurred while fetching the EUR to CZK rate: {ex.Message}", ex);
        }
    }
}