namespace DemoBackShopCore.Utils
{
    public class DomainResponseMessages
    {
        //Customers
        public const string CustomerCustomerIdMustBeGreaterThanZeroError = "CustomerId must be greater than zero"; 
        public const string MaximumOf40CharactersError = "Must have a maximum of 40 characters";
        public const string DateOfBirthError = "You cannot put the date with the day after today.";
        public const string CustomerFieldsAreInvalidError = "FirstName, Lastname or DateOfBirth fields are invalid, check the values available.";
    }
}