namespace DemoBackShopCore.Utils
{
    public class DomainResponseMessages
    {
        //Customers
        public const string CustomerCustomerIdMustBeGreaterThanZeroError = "CustomerId must be greater than zero"; 
        public const string MaximumOf40CharactersError = "Must have a maximum of 40 characters";
        public const string DateOfBirthError = "You cannot put the date with the day after today.";
        public const string CustomerFieldsAreInvalidError = "FirstName, Lastname or DateOfBirth fields are invalid, check the values available.";
        public const string EmailFieldIsNotAValid = "The Email field is not a valid e-mail address.";
        public const string FirstNameIsRequired = "FirstName Is Required";
        public const string LastNameIsRequired = "LastName Is Required";
        public const string EmailAddressIsRequired = "EmailAddress Is Required";
        public const string DateOfBirthIsRequired = "DateOfBirth Is Required";
        public const string CustomerPaginationError = "The pagination parameters 'pageSize' and 'pageNumber' must be positive numbers. Check the values ​​provided.";
    }
}