namespace DemoBackShopCore.DTOs
{
    public class CustomerDTOWithMessageErrors
    {
        public CustomerRequestDTO Customer { get; set; } = new CustomerRequestDTO();
        public List<string> FailureErrorsMessages { get; set; } = new List<string>();
    }
}