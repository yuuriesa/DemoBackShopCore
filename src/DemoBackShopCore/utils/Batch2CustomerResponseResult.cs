using DemoBackShopCore.DTOs;

namespace DemoBackShopCore.Utils
{
    public class Batch2CustomerResponseResult
    {
        public int CustomersSuccessCount { get; set; }
        public int CustomersFailureCount { get; set; }
        public List<CustomerRequestDTO>? Success { get; set; } = new List<CustomerRequestDTO>();
        public List<CustomerDTOWithMessageErrors>? Failure { get; set; } = new List<CustomerDTOWithMessageErrors>();
    }
}