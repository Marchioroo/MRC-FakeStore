namespace FakeStore.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int? ExternalId { get; set; }
    public string Title { get; set; } = String.Empty; 
    public string Barcode { get; set; } = String.Empty;
    public decimal Price { get; set; }  
    public string Description { get; set; } = String.Empty; 
    public string Category { get; set; } = String.Empty; 
    public string Image { get; set; } = String.Empty; 
}
