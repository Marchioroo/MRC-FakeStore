using FakeStore.Application.UseCases.Products.GenerateBarcode;
using FakeStore.Communication.Responses;
using FakeStore.Domain.Entities;
using FakeStore.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


public class FakeStoreService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly BarcodeGenerator _barcodeGenerator;


    public FakeStoreService(HttpClient httpClient, AppDbContext context, BarcodeGenerator barcodeGenerator)
    {
        _httpClient = httpClient;
        _context = context;
        _barcodeGenerator = barcodeGenerator;
    }

    public async Task<List<Product>> SyncAndGetAllProductsAsync()
    {
        // 1. Consumir a API FakeStore
        var response = await _httpClient.GetStringAsync("https://fakestoreapi.com/products");

        var productsFromApi = JsonConvert.DeserializeObject<List<ExternalProductJson>>(response);

        if (productsFromApi == null || productsFromApi.Count == 0)
        {
            throw new Exception("Nenhum produto foi retornado pela API.");
        }

        // 3. Buscar ExternalIds já existentes no banco
        var existingExternalIds = await _context.Products
            .Where(p => p.ExternalId != null)
            .Select(p => p.ExternalId.Value)
            .ToListAsync();

        // 4. Filtrar apenas os novos produtos
        var newProducts = productsFromApi
     .Where(p => !existingExternalIds.Contains(p.Id))
     .Select(p => new Product
     {
         ExternalId = p.Id,
         Title = p.Title,
         Price = p.Price,
         Description = p.Description,
         Category = p.Category,
         Image = p.Image,
         Barcode = _barcodeGenerator.Generate() // Gera um código de barras
     })
     .ToList();

        if (newProducts.Any())
        {
            _context.Products.AddRange(newProducts);
            await _context.SaveChangesAsync();
        }

        // 5. Retornar todos os produtos do banco
        return await _context.Products.ToListAsync();
    }

    public async Task<PagedResponse<Product>> GetPagedProductsAsync(int pageNumber, int pageSize, string? name = null, string? barcode = null)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Title.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(barcode))
        {
            query = query.Where(p => p.Barcode.Contains(barcode));
        }

        var totalRecords = await query.CountAsync();

        var data = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<Product>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Data = data
        };
    }




}
