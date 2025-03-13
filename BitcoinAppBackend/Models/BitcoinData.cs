using System;

namespace BitcoinAppBackend.Models;

public class BitcoinData
{
    public int Id { get; set; }
    public decimal PriceEUR { get; set; }
    public decimal PriceCZK { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Note { get; set; } // Editable note column
}


