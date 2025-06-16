namespace FakeStore.Domain.Entities;
public class ExternalProductJson
{
    public int Id { get; set; }  // Id da API externa
    public string Title { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = String.Empty;
    public string Category { get; set; } = String.Empty;
    public string Image { get; set; } = String.Empty;
}
