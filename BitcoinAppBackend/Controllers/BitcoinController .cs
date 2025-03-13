using BitcoinAppBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BitcoinAppBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitcoinController : ControllerBase
{
    private readonly IBitcoinPriceCalculatorService _calculatorService;

    public BitcoinController(IBitcoinPriceCalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    // GET: api/bitcoin/price
    [HttpGet("price")]
    public async Task<IActionResult> GetBitcoinPriceAsync()
    {
        try
        {
            var (bitcoinPriceEUR, bitcoinPriceCZK) = await _calculatorService.GetBitcoinPriceAsync();

            // Vracíme objekt s cenami
            return Ok(new
            {
                BitcoinPriceEUR = bitcoinPriceEUR,
                BitcoinPriceCZK = bitcoinPriceCZK
            });
        }
        catch (Exception ex)
        {
            // Ošetření chyb
            return BadRequest(new { message = ex.Message });
        }
    }
}
