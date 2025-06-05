using DemoBackShopCore.DTOs;

namespace DemoBackShopCore.Utils
{
    public class Batch2ResponseResult
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<ProductRequestDTO>? Success { get; set; } = new List<ProductRequestDTO>();
        public List<ProductDTOWithMessageErrors>? Failure { get; set; } = new List<ProductDTOWithMessageErrors>();
    }
}