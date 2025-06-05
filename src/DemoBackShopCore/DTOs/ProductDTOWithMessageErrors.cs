namespace DemoBackShopCore.DTOs
{
    public class ProductDTOWithMessageErrors
    {
        public ProductRequestDTO Product { get; set; } = new ProductRequestDTO();
        public List<string> FailureErrorsMessages { get; set; } = new List<string>();
    }
}