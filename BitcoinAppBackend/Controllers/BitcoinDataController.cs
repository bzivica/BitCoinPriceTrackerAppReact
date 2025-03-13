using Microsoft.AspNetCore.Mvc;
using BitcoinAppBackend.Data;
using BitcoinAppBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinAppBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitcoinDataController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BitcoinDataController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/BitcoinData
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BitcoinData>>> GetBitcoinData()
    {
        return await _context.BitcoinData.ToListAsync();
    }

    // POST: api/BitcoinData
    [HttpPost]
    public async Task<ActionResult<BitcoinData>> PostBitcoinData(BitcoinData bitcoinData)
    {
        _context.BitcoinData.Add(bitcoinData);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetBitcoinData", new { id = bitcoinData.Id }, bitcoinData);
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<IEnumerable<BitcoinData>>> PostBulkBitcoinData(List<BitcoinData> bitcoinDataList)
    {
        if (bitcoinDataList == null || bitcoinDataList.Count == 0)
        {
            return BadRequest("No data provided.");
        }

        // Přidáme všechny záznamy do databáze
        _context.BitcoinData.AddRange(bitcoinDataList);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Pokud dojde k chybě při ukládání, vrátíme chybu
            return StatusCode(500, $"Error saving data: {ex.Message}");
        }

        // Vracíme úspěšně přidané záznamy
        return Ok(bitcoinDataList);
    }
    // PUT: api/BitcoinData/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBitcoinData(int id, BitcoinData bitcoinData)
    {
        if (id != bitcoinData.Id)
        {
            return BadRequest();
        }

        _context.Entry(bitcoinData).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/BitcoinData/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBitcoinData(int id)
    {
        var bitcoinData = await _context.BitcoinData.FindAsync(id);
        if (bitcoinData == null)
        {
            return NotFound();
        }

        _context.BitcoinData.Remove(bitcoinData);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

