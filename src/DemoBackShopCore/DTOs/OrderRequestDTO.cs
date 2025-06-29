using System.ComponentModel.DataAnnotations;
using DemoBackShopCore.Models;

namespace DemoBackShopCore.DTOs
{
    public class OrderRequestDTO
    {
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Required]
        public CustomerRequestDTOForOrder Customer { get; set; }
        public List<ItemRequestDTO> Items { get; set; } = new List<ItemRequestDTO>();
    }
}