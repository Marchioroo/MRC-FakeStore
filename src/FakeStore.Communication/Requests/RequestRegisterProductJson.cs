using Swashbuckle.AspNetCore.Annotations;

namespace FakeStore.Communication.Requests;
public class RequestRegisterProductJson
{
    public int Id { get; set; }  // O EF Core vai gerar automaticamente o valor para o Id

    [SwaggerSchema("Título do produto")]
    public string Title { get; set; } = String.Empty; // Corresponde ao campo 'title' da API

    [SwaggerSchema("Preço do produto")]
    public decimal Price { get; set; }  // Corresponde ao campo 'price' da API

    [SwaggerSchema("Descrição completa do produto")]
    public string Description { get; set; } = String.Empty; // Corresponde ao campo 'description' da API

    [SwaggerSchema("Categoria do produto")]
    public string? Category { get; set; } = String.Empty; // Corresponde ao campo 'category' da API

    [SwaggerSchema("Barcode, se não for enviado é gerado um aleatório")]
    public string? Barcode { get; set; } = string.Empty;

    [SwaggerSchema("Envie uma imagem do tipo file")]
    public string Image { get; set; } = String.Empty; // Corresponde ao campo 'image' da API
}
