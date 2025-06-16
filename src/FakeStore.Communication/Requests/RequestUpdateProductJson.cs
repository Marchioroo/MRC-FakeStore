using Microsoft.AspNetCore.Http;

namespace FakeStore.Communication.Requests
{
    public class RequestUpdateProductJson
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public string? Category { get; set; }
        public IFormFile? Image { get; set; } 
    }
}
