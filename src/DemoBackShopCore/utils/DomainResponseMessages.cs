namespace DemoBackShopCore.Utils
{
    public class DomainResponseMessages
    {
        //Customers
        public const string CustomerCustomerIdMustBeGreaterThanZeroError = "CustomerId must be greater than zero";
        public const string CustomerNotFoundMessageError = "Customer not found";
        public const string MaximumOf40CharactersError = "Must have a maximum of 40 characters";
        public const string DateOfBirthError = "You cannot put the date with the day after today.";
        public const string CustomerFieldsAreInvalidError = "FirstName, Lastname or DateOfBirth fields are invalid, check the values available.";
        public const string EmailFieldIsNotAValid = "The Email field is not a valid e-mail address.";
        public const string FirstNameIsRequired = "FirstName Is Required";
        public const string LastNameIsRequired = "LastName Is Required";
        public const string EmailAddressIsRequired = "EmailAddress Is Required";
        public const string DateOfBirthIsRequired = "DateOfBirth Is Required";
        public const string CustomerEmailExistsError = "this email exists";
        public const string DuplicateEmailError = "found duplicate emails";
        public const string CustomerPaginationError = "The pagination parameters 'pageSize' and 'pageNumber' must be positive numbers. Check the values ​​provided.";
    
    
        //Address
        public const string MaximumOf20CharactersError = "Must have a maximum of 20 characters";
        public const string MaximumOf100CharactersError = "Must have a maximum of 100 characters";
        public const string MaximumOf50CharactersError = "Must have a maximum of 50 characters";
        public const string MustHaveAtLeastOneAddress = "You must have at least one address";

        //Product
        public const string ProductIdMustBeGreaterThanZeroError = "ProductId must be greater than zero";
        public const string ProductNotFoundMessageError = "Product not found";
        public const string CodeIsRequired = "Code Is Required";
        public const string ProductCodeExistsError = "this Code exists";
        public const string ProductNameIsRequired = "ProductName Is Required";
        public const string ProductFieldsAreInvalidError = "Code or Name fields are invalid, check the values available.";
    }
}