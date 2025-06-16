

using FakeStore.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace FakeStore.Communication.Responses;

public class ResponseRegisterProductJson
{
    [SwaggerSchema("ID gerado do produto")]
    public int Id { get; set; }

    [SwaggerSchema("Título do produto")]
    public string Title { get; set; }

    [SwaggerSchema("Preço do produto")]
    public decimal Price { get; set; }

    [SwaggerSchema("Descrição do produto")]
    public string Description { get; set; }

    [SwaggerSchema("Categoria do produto")]
    public string Category { get; set; }

    [SwaggerSchema("Imagem do produto")]
    public string Image { get; set; }

    [SwaggerSchema("Código de barras do produto")]
    public string Barcode { get; set; } = string.Empty;


    public ResponseRegisterProductJson(Product product)
    {
        Id = product.Id;
        Title = product.Title;
        Barcode = product.Barcode;
        Price = product.Price;
        Description = product.Description;
        Category = product.Category;
        Image = product.Image;
    }
}
