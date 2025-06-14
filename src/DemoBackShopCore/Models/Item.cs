using System.Text.Json.Serialization;

namespace DemoBackShopCore.Models
{
    public class Item
    {
        //private properties
        private int _quantityOfItems { get; set; }
        private decimal _unitValue { get; set; }


        //public properties
        public int ItemId { get; private set; }
        public int QuantityOfItems => _quantityOfItems;
        public decimal UnitValue => _unitValue;
        public decimal TotalValue { get; private set; }
        public int ProductId { get; private set; }
        public int OrderId { get; private set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public string Code { get; private set; }

        //properties for validation
        public bool IsValid { get; private set; }
    }
}