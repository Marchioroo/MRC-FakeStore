using FakeStore.Domain.Entities;
using FakeStore.Infrastructure.DataAccess;
using FakeStore.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

namespace FakeStore.Application.UseCases.Products.Delete;

public class DeleteProductUseCase
{
    private readonly AppDbContext _context;
    private readonly AzureBlobStorageService _blobService;

    public DeleteProductUseCase(AppDbContext context, AzureBlobStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return false;

        // Se o produto tem uma imagem no Azure, tenta excluir
        if (!string.IsNullOrEmpty(product.Image) && product.Image.Contains(".blob.core.windows.net"))
        {
            await _blobService.DeleteImageAsync(product.Image);
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}