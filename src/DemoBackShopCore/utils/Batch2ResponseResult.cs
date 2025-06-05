using DemoBackShopCore.DTOs;

namespace DemoBackShopCore.Utils
{
    public class Batch2ResponseResult
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<CustomerRequestDTO>? Success { get; set; } = new List<CustomerRequestDTO>();
        public List<CustomerDTOWithMessageErrors>? Failure { get; set; } = new List<CustomerDTOWithMessageErrors>();
    }
}