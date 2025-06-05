using DemoBackShopCore.DTOs;

namespace DemoBackShopCore.Utils
{
    public class Batch2PreparedForReponse
    {
        // This class is only used to serve as a response from the other Batch2ReponseResult class,
        // to have a nice return with the IDs of the customers that are successful and those that fail,
        // both will have their dates without the times that are standard.
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<ProductRequestDTO>? Success { get; set; } = new List<ProductRequestDTO>();
        public List<ProductDTOWithMessageErrors>? Failure { get; set; } = new List<ProductDTOWithMessageErrors>();
    }
}