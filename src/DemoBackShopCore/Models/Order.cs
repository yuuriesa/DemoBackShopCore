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
        private Order
        (
            int orderId,
            string orderNumber,
            DateTime orderDate,
            decimal totalOrderValue,
            int customerId,
            List<Item> items
        )
        {
            OrderId = orderId;
            _orderNumber = orderNumber;
            _orderDate = orderDate;
            _totalOrderValue = totalOrderValue;
            _customerId = customerId;
            Items = items;

            Validate();
        }

        //public methods
        public Order RegisterNew
        (
            string orderNumber,
            DateTime orderDate,
            int customerId,
            List<Item> items
        )
        {
            Order order = new Order();

            order.SetOrderNumber(orderNumber: orderNumber);
            order.SetOrderDate(orderDate: orderDate);
            order.SetOrderCustomerId(customerId: customerId);
            order.SetOrderItems(items: items);

            return order;
        }

        public Order SetExistingInfo
        (
            int orderId,
            string orderNumber,
            DateTime orderDate,
            decimal totalOrderValue,
            int customerId,
            List<Item> items
        )
        {
            Order order = new Order
            (
                orderId: orderId,
                orderNumber: orderNumber,
                orderDate: orderDate,
                totalOrderValue: totalOrderValue,
                customerId: customerId,
                items: items
            );

            return order;
        }


        //private methods
        private void SetOrderNumber(string orderNumber)
        {

        }
        private void SetOrderDate(DateTime orderDate)
        {
            
        }
        private void SetOrderCustomerId(int customerId)
        {
            
        }
        private void SetOrderItems(List<Item> items)
        {
            
        }
        private void Validate()
        {

        }
    }
}