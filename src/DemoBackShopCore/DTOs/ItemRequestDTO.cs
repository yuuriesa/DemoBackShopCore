namespace DemoBackShopCore.DTOs
{
    public class ItemRequestDTO
    {
        public ProductRequestDTO Product { get; set; }
        public int QuantityOfItems { get; set; }
        public decimal UnitValue { get; set; }
    }
}