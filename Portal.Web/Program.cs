using Microsoft.EntityFrameworkCore;
using Portal.Application.AppServices;
using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Validators;
using Portal.Infra.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PortalDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

builder.Services.AddScoped<IVendedorAppService, VendedorAppService>();
builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

var app = builder.Build();


app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (BusinessException ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Erro Crítico]: {ex.Message}"); //Criar log apartir daqui
        
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";  
        await context.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro interno inesperado." });
    }
});

//if (app.Environment.IsDevelopment())
//{
//app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("BlazorPolicy");

try //To do.: retirar o try catch depois de testar via swagger.
{
    app.MapControllers();
}
catch (System.Reflection.ReflectionTypeLoadException ex)
{
    foreach (var le in ex.LoaderExceptions)
    {
        Console.WriteLine(le?.Message);
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
    //db.Database.Migrate(); //To do.: Rodar migrations
}

app.Run();
