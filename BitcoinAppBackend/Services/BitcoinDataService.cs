
using System;
using System.Threading.Tasks;
using BitcoinAppBackend.Data;
using BitcoinAppBackend.Models;

namespace BitcoinAppBackend.Services;

public class BitcoinDataService
{
    private readonly ApplicationDbContext _context;

    public BitcoinDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Method to save Bitcoin data into the database
    public async Task SaveBitcoinDataAsync(decimal priceEUR, decimal priceCZK, string note)
    {
        var bitcoinData = new BitcoinData
        {
            PriceEUR = priceEUR,
            PriceCZK = priceCZK,
            Timestamp = DateTime.Now,
            Note = note
        };

        _context.BitcoinData.Add(bitcoinData);
        await _context.SaveChangesAsync();
    }
}
