//using Ecommerce.Repository;
using Ordini.Business;
using Ordini.Business.Abstraction;
using Ordini.Repository;
using Ordini.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Magazzino.ClientHttp.Abstraction;
using Magazzino.ClientHttp;




var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(a =>
{
    a.ListenAnyIP(80);
});

builder.Services.AddControllers();
builder.Services.AddDbContext<OrdiniDbContext>(options => options.UseSqlServer("Server=mssql-server;Database=Ordini;User Id=sa;Password=p4ssw0rD;Encrypt=False"));

//setup clientHttp
IConfigurationSection config = builder.Configuration.GetSection(MagazzinoClientOptions.SectionName);

MagazzinoClientOptions optionsInventario = config.Get<MagazzinoClientOptions>() ?? new();

builder.Services.AddHttpClient<IClientMagazzino, ClientMagazzino>("ClientMagazzino", client =>
{
    client.BaseAddress = new Uri("http://magazzino_service:80");
});

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
