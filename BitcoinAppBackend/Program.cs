
using Microsoft.EntityFrameworkCore;
using BitcoinAppBackend.Services.Interfaces;
using BitcoinAppBackend.Services;
using BitcoinAppBackend.Data;
using Microsoft.AspNetCore.Hosting;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddScoped<IBitcoinPriceCalculatorService, BitcoinPriceCalculatorService>();


builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICoindeskService, CoindeskService>();
builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>();
builder.Services.AddHttpClient<ICnbService, CnbService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
