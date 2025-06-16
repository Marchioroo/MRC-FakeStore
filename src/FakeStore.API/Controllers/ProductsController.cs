using FakeStore.Application.UseCases.Products.Delete;
using FakeStore.Application.UseCases.Products.GenerateBarcode;
using FakeStore.Application.UseCases.Products.Register;
using FakeStore.Application.UseCases.Products.Update;
using FakeStore.Communication.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly FakeStoreService _fakeStoreService;
    private readonly RegisterProductUseCase _registerProductUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;


    public ProductsController(FakeStoreService fakeStoreService, RegisterProductUseCase registerProductUseCase, DeleteProductUseCase deleteProductUseCase, UpdateProductUseCase updateProductUseCase)
    {
        _fakeStoreService = fakeStoreService;
        _registerProductUseCase = registerProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
        _updateProductUseCase = updateProductUseCase;
    }

    [HttpPost("sync")]
    [SwaggerOperation(Summary = "Sincroniza os produtos da FakeStore com o banco de dados!")]
    public async Task<IActionResult> SyncFakeStore()
    {
        await _fakeStoreService.SyncAndGetAllProductsAsync();
        return Ok(new { Message = "Produtos sincronizados com sucesso." });
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Traz todos os produtos do banco de dados!")]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _fakeStoreService.GetPagedProductsAsync(pageNumber, pageSize);
        return Ok(response);
    }


    //[HttpPost]
    //public async Task<IActionResult> Register([FromBody] RequestRegisterProductJson request)
    //{
    //    var response = await _registerProductUseCase.ExecuteAsync(request);
    //    return CreatedAtAction(nameof(GetProducts), new { id = response.Id }, response);
    //}

    [HttpPost("register-with-image")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(Summary = "Cadastrar um novo produto", Description = "Cria um novo produto na loja")]
    public async Task<IActionResult> RegisterWithImage([FromForm] RequestRegisterProductWithImage request)
    {
        using var ms = new MemoryStream();
        await request.Image.CopyToAsync(ms);
        var imageBytes = ms.ToArray();

        var requestProduct = new RequestRegisterProductJson
        {
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category ?? "Sem categoria",
            Image = string.Empty 
        };

        var response = await _registerProductUseCase.ExecuteAsync(requestProduct, imageBytes);

        return CreatedAtAction(nameof(GetProducts), new { id = response.Id }, response);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletar um produto já existente!")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _deleteProductUseCase.ExecuteAsync(id);
        if (!deleted)
            return NotFound(new { Error = "Produto não encontrado." });
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(Summary = "Alterar um produto já existente!")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] RequestUpdateProductJson request)
    {
        var response = await _updateProductUseCase.ExecuteAsync(id, request);

        if (response == null)
            return NotFound(new { Error = "Produto não encontrado." });

        return Ok(response);
    }


}
