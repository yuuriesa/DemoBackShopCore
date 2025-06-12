using System.Text.Json.Serialization;

namespace DemoBackShopCore.Models
{
    public class Order
    {
        //private properties
        private string _orderNumber { get; set; }
        private DateTime _orderDate { get; set; }
        private decimal _totalOrderValue { get; set; }
        private int _customerId { get; set; }

        //public properties
        public int OrderId { get; private set; }
        public string OrderNumber => _orderNumber;
        public DateTime OrderDate => _orderDate;
        public decimal TotalOrderValue => _totalOrderValue;
        public int CustomerId => _customerId;
        [JsonIgnore]
        public Customer Customer { get; set; }
        public ICollection<Item> Items { get; private set; } = new List<Item>();

        //properties for validation
        public bool IsValid { get; private set; } = false;
        public string ErrorMessageIfIsNotValid { get; private set; } = string.Empty;

        //Private Constructors
        private Order()
        {

        }
    }
}