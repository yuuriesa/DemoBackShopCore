namespace DemoBackShopCore.DTOs
{
    public class CustomerResponseDTO
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public List<AddressResponseDTO> Addresses { get; set; }
    }
}