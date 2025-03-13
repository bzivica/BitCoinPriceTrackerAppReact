using Microsoft.EntityFrameworkCore;
using BitcoinAppBackend.Models;
using System.Collections.Generic;

namespace BitcoinAppBackend.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<BitcoinData> BitcoinData { get; set; }
}
