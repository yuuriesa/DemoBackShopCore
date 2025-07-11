using System.ComponentModel.DataAnnotations;

namespace DemoBackShopCore.DTOs
{
    public class OrderResponseDTO
    {
        public int orderId { get; set; }
        public string orderNumber { get; set; }
        public string orderDate { get; set; }
        public decimal totalOrderValue { get; set; }
        public CustomerResponseDTO customer { get; set; }
        public List<ItemResponseDTO> items { get; set; } = new List<ItemResponseDTO>();
    }
}