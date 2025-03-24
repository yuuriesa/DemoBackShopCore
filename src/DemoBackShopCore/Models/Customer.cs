using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Models
{
    public class Customer
    {
        public static int MAXIMUM_CHARACTERS_FIRST_NAME = 40;
        public static int MAXIMUM_CHARACTERS_LAST_NAME = 40;

        //private properties
        private string _firstName { get; set; }
        private string _lastName { get; set; }
        private string _emailAddress { get; set; }
        private DateOnly _dateOfBirth { get; set; }
        private bool _isValid { get; set; }

        //public properties
        public int CustomerId { get; private set; }
        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string EmailAddress => _emailAddress;
        public DateOnly DateOfBirth => _dateOfBirth;

        //constructors
        private Customer()
        {
            
        }
        private Customer(int customerId, string firstName, string lastName, string emailAddress, DateOnly dateOfBirth)
        {
            SetCustomerId(customerId: customerId);
            SetFirstName(firstName: firstName);
            SetLastName(lastName: lastName);
            SetEmailAddress(emailAddress: emailAddress);
            SetDateOfBirth(dateOfBirth: dateOfBirth.ToDateTime(TimeOnly.MinValue));
            Validate(firstName: firstName, lastName: lastName, dateOfBirth: dateOfBirth.ToDateTime(TimeOnly.MinValue));
        }

        //public methods
        public static Customer RegisterNew(string firstName, string lastName, string emailAddress, DateTime dateOfBirth)
        {
            var customer = new Customer();
            customer.SetFirstName(firstName: firstName);
            customer.SetLastName(lastName: lastName);
            customer.SetEmailAddress(emailAddress: emailAddress);
            customer.SetDateOfBirth(dateOfBirth: dateOfBirth);
            customer.Validate(firstName: firstName, lastName: lastName, dateOfBirth: dateOfBirth);

            return customer;
        }

        public static Customer SetExistingInfo(int customerId, string firstName, string lastName, string emailAddress, DateOnly dateOfBirth)
        {
            var customer = new Customer(customerId: customerId, firstName: firstName, lastName: lastName, emailAddress: emailAddress, dateOfBirth: dateOfBirth);
            return customer;
        }

        public bool IsValid()
        {
            return _isValid;
        }

        //private methods
        private void SetCustomerId(int customerId)
        {
            if (customerId < 1)
            {
                throw new ArgumentOutOfRangeException(DomainResponseMessages.CustomerCustomerIdMustBeGreaterThanZeroError);
            }
            CustomerId = customerId;
        }

        private void SetFirstName(string firstName)
        {
            if (firstName.Length > MAXIMUM_CHARACTERS_FIRST_NAME)
            {
                throw new ArgumentOutOfRangeException(DomainResponseMessages.MaximumOf40CharactersError);
            }

            _firstName = firstName;
        }

        private void SetLastName(string lastName)
        {
            if (lastName.Length > MAXIMUM_CHARACTERS_LAST_NAME)
            {
                throw new ArgumentOutOfRangeException(DomainResponseMessages.MaximumOf40CharactersError);
            }

            _lastName = lastName;
        }

        private void SetEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
        }

        private void SetDateOfBirth(DateTime dateOfBirth)
        {
            var dateNow = DateTime.Now;

            if (dateOfBirth.ToUniversalTime().Date > dateNow.Date)
            {
                throw new ArgumentOutOfRangeException(DomainResponseMessages.DateOfBirthError);
            }

            _dateOfBirth = DateOnly.FromDateTime(dateTime: dateOfBirth);
        }

        private void Validate(string firstName, string lastName, DateTime dateOfBirth)
        {
            _isValid = false;
            
            var dateNow = DateTime.Now;
            
            if (firstName.Length > MAXIMUM_CHARACTERS_FIRST_NAME || lastName.Length > MAXIMUM_CHARACTERS_LAST_NAME || dateOfBirth.ToUniversalTime().Date > dateNow.Date)
            {
                throw new ArgumentOutOfRangeException(DomainResponseMessages.CustomerFieldsAreInvalidError);
            }

            _isValid = true;
        }
    }
}