using Microsoft.EntityFrameworkCore;
using Portal.Infra.Data.Repository;
using Portal.Application.Interfaces;
using Portal.Application.AppServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Register controllers and EF Core DbContext
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PortalDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVendedorAppService, VendedorAppService>();
builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();
//builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

try
{
    app.MapControllers();
}
catch (System.Reflection.ReflectionTypeLoadException ex)
{
    foreach (var le in ex.LoaderExceptions)
    {
        Console.WriteLine(le?.Message); // ISSO vai te dizer qual DLL está faltando
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
    db.Database.Migrate(); // Aplica migrations pendentes automaticamente
}

app.Run();
