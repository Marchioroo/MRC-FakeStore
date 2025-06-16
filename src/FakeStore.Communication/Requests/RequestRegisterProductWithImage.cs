using Microsoft.AspNetCore.Http;

namespace FakeStore.Communication.Requests
{
    public class RequestRegisterProductWithImage
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public IFormFile Image { get; set; }  
    }
}
