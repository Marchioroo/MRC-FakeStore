using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace FakeStore.Communication.Requests
{
    public class RequestRegisterProductWithImage
    {
        [SwaggerSchema("Título do produto")]
        public string Title { get; set; } = string.Empty;

        [SwaggerSchema("Preço do produto")]
        public decimal Price { get; set; }

        [SwaggerSchema("Descrição do produto")]
        public string Description { get; set; } = string.Empty;

        [SwaggerSchema("Categoria do produto (opcional)")]
        public string? Category { get; set; } = string.Empty;

        [SwaggerSchema("Código de barras do produto (opcional). Se não enviado, será gerado automaticamente")]
        public string? Barcode { get; set; } = string.Empty;

        [SwaggerSchema("Imagem do produto (arquivo do tipo image)")]
        public IFormFile Image { get; set; }
    }
}
