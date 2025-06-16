using FakeStore.Application.UseCases.Products.Register;
using FakeStore.Communication.Requests;
using FakeStore.Communication.Responses;
using FakeStore.Exception.ExceptionBase;
using FakeStore.Infrastructure.DataAccess;
using FakeStore.Infrastructure.Storage;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FakeStore.Application.UseCases.Products.Update;

public class UpdateProductUseCase
{
    private readonly AppDbContext _context;
    private readonly AzureBlobStorageService _blobService;

    public UpdateProductUseCase(AppDbContext context, AzureBlobStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    public async Task<ResponseRegisterProductJson?> ExecuteAsync(int id, RequestUpdateProductJson request)
    {

        Validate(request);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return null;

        // Se veio uma nova imagem, faz upload e substitui
        if (request.Image != null)
        {
            // Deleta imagem antiga, se existir
            if (!string.IsNullOrEmpty(product.Image) && product.Image.Contains(".blob.core.windows.net"))
            {
                await _blobService.DeleteImageAsync(product.Image);
            }

            // Faz upload da nova imagem
            using var ms = new MemoryStream();
            await request.Image.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            var newImageUrl = await _blobService.UploadImageAsync(imageBytes, $"product_update_{Guid.NewGuid()}.jpg");
            product.Image = newImageUrl;
        }

        // Atualiza os outros Title
        if (!string.IsNullOrWhiteSpace(request.Title))
            product.Title = request.Title;

        if (request.Price.HasValue)
            product.Price = request.Price.Value;

        if (!string.IsNullOrWhiteSpace(request.Description))
            product.Description = request.Description;

        if (!string.IsNullOrWhiteSpace(request.Category))
            product.Category = request.Category;

        if (!string.IsNullOrWhiteSpace(request.Barcode))
            product.Barcode = request.Barcode;


        await _context.SaveChangesAsync();

        return new ResponseRegisterProductJson(product);
    }

    private void Validate(RequestUpdateProductJson request)
    {
        if (request == null)
            throw new ArgumentException(nameof(request));

        var validator = new UpdateProductValidator();  
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
