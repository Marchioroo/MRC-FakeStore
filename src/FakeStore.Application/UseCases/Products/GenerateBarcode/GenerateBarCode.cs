namespace FakeStore.Application.UseCases.Products.GenerateBarcode
{
    public class BarcodeGenerator
    {
        private static readonly Random _random = new();

        public string Generate()
        {
            // Gera um número de 12 dígitos (exemplo comum de tamanho de código de barras)
            return _random.Next(100000000, 999999999).ToString();
        }
    }
}
