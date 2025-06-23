using System.ComponentModel.DataAnnotations;
using DemoBackShopCore.Models;

namespace DemoBackShopCore.DTOs
{
    public class OrderRequestDTO
    {
        public string OrderNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public CustomerRequestDTO Customer { get; set; }
        public List<ItemRequestDTO> Items { get; set; } = new List<ItemRequestDTO>();
    }
}