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
        public bool IsValid { get; private set; } = false;
        public string ErrorMessageIfIsNotValid { get; private set; } = string.Empty;

        //constructors
        //private constructor
        private Item() { }
        private Item
        (
            int itemId,
            Product product,
            int orderId,
            int quantityOfItems,
            decimal unitValue,
            string code
        )
        {
            ItemId = itemId;
            ProductId = product.ProductId;
            OrderId = orderId;
            _quantityOfItems = quantityOfItems;
            _unitValue = unitValue;
            Code = code;
        }

        //public methods
        public static Item RegisterNew
        (
            Product product,
            decimal unitValue,
            int quantityOfItems
        )
        {
            Item item = new Item();
            item.SetProduct(product: product);
            item.SetUnitValue(unitValue: unitValue);
            item.SetQuantityOfItems(quantityOfItems: quantityOfItems);
            item.SetTotalValue();
            item.Validate();

            return item;
        }

        public static Item SetExistingInfo
        (
            int itemId,
            Product product,
            int orderId,
            int quantityOfItems,
            decimal unitValue,
            string code
        )
        {
            Item item = new Item
            (
                itemId: itemId,
                product: product,
                orderId: orderId,
                quantityOfItems: quantityOfItems,
                unitValue: unitValue,
                code: code
            );

            item.Validate();

            return item;
        }

        //private methods
        private void SetUnitValue(decimal unitValue)
        {
            if (unitValue <= 0)
            {
                ErrorMessageIfIsNotValid = "";
            }

            _unitValue = unitValue;
        }

        private void SetQuantityOfItems(int quantityOfItems)
        {
            if (quantityOfItems <= 0)
            {
                ErrorMessageIfIsNotValid = "";
            }

            _quantityOfItems = quantityOfItems;
        }

        private void SetProduct(Product product)
        {
            ProductId = product.ProductId;
            Code = product.Code;
        }

        private void SetTotalValue()
        {
            TotalValue = _unitValue * _quantityOfItems;
        }

        private void Validate()
        {
            IsValid = _unitValue > 0 && _quantityOfItems > 0;
        }
    }
}