using CashFlow.Api.Filters;
using FakeStore.Application.UseCases.Products.Delete;
using FakeStore.Application.UseCases.Products.GenerateBarcode;
using FakeStore.Application.UseCases.Products.Register;
using FakeStore.Application.UseCases.Products.Update;
using FakeStore.Infrastructure.DataAccess;
using FakeStore.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Models;
using DotNetEnv;

// Carregar variáveis de ambiente do .env (ignorar erro se não existir)
try
{
    Env.Load();
}
catch { }

// Sobrescrever ConnectionStrings
var builder = WebApplication.CreateBuilder(args);

builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
builder.Configuration["ConnectionStrings:AzureBlobStorage"] = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE");

// Configuração dos serviços
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RegisterProductUseCase>();
builder.Services.AddSingleton<BarcodeGenerator>();
builder.Services.AddSingleton(new AzureBlobStorageService(
    builder.Configuration.GetConnectionString("AzureBlobStorage")));

builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddHttpClient<FakeStoreService>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.AddControllers();

var app = builder.Build();

// Pipeline HTTP + Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
