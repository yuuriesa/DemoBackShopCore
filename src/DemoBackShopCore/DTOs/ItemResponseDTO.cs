namespace DemoBackShopCore.DTOs
{
    public class ItemResponseDTO
    {
        public int itemId { get; set; }
        public int orderId { get; set; }
        public int quantityOfItems { get; set; }
        public decimal unitValue { get; set; }
        public ProductResponseDTO product { get; set; }
    }
}