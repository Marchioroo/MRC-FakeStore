using FakeStore.Application.UseCases.Products.GenerateBarcode;
using FakeStore.Communication.Requests;
using FakeStore.Communication.Responses;
using FakeStore.Domain.Entities;
using FakeStore.Exception.ExceptionBase;
using FakeStore.Infrastructure.DataAccess;
using FakeStore.Infrastructure.Storage;

namespace FakeStore.Application.UseCases.Products.Register;

public class RegisterProductUseCase
{
    private readonly AppDbContext _context;
    private readonly AzureBlobStorageService _blobService;
    private readonly BarcodeGenerator _barcodeGenerator;

    public RegisterProductUseCase(AppDbContext context, AzureBlobStorageService blobService, BarcodeGenerator barcodeGenerator)
    {
        _context = context;
        _blobService = blobService;
        _barcodeGenerator = barcodeGenerator;
    }

    public async Task<ResponseRegisterProductJson> ExecuteAsync(RequestRegisterProductJson request, byte[] imageBytes)
    {
        Validate(request);

        string imageUrl = await _blobService.UploadImageAsync(imageBytes, $"product_{Guid.NewGuid()}.jpg");

        var product = new Product
        {
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category ?? "Sem categoria",
            Barcode = request.Barcode ?? _barcodeGenerator.Generate(),
            Image = imageUrl  // Salva a URL no banco
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return new ResponseRegisterProductJson(product);
    }

    private void Validate(RequestRegisterProductJson request)
    {
        if (request == null)
            throw new ArgumentException(nameof(request));

        var validator = new RegisterProductValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
