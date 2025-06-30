namespace DemoBackShopCore.DTOs
{
    public class OrderResponseDTO
    {
        public int orderId { get; set; }
        public string orderNumber { get; set; }
        public DateTime orderDate { get; set; }
        public decimal totalOrderValue { get; set; }
        public CustomerResponseDTO customer { get; set; }
        //List<ItemResponseDTO> items { get; set; } = new List<ItemResponseDTO>();
    }
}