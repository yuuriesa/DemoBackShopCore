using DemoBackShopCore.DTOs;

namespace DemoBackShopCore.Utils
{
    public class Batch2PreparedForCustomerReponse
    {
        // This class is only used to serve as a response from the other Batch2CustomerReponseResult class,
        // to have a nice return with the IDs of the customers that are successful and those that fail,
        // both will have their dates without the times that are standard.
        public int CustomersSuccessCount { get; set; }
        public int CustomersFailureCount { get; set; }
        public List<CustomerResponseDTO>? Success { get; set; } = new List<CustomerResponseDTO>();
        public List<CustomerDTOWithMessageErrors>? Failure { get; set; } = new List<CustomerDTOWithMessageErrors>();
    }
}