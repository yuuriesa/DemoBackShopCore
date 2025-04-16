using DemoBackShopCore.DTOs;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Models
{
    public class Customer
    {
        //private properties
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private DateOnly _dateOfBirth;
        private ICollection<Address> _addresses;
        private bool _isValid { get; set; } = false;

        //public properties
        public int CustomerId { get; private set; }
        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string EmailAddress => _emailAddress;
        public DateOnly DateOfBirth => _dateOfBirth;
        public ICollection<Address> Addresses => _addresses;
        public string ErrorMessageIfIsNotValid { get; private set; } = string.Empty;

        //constructors
        private Customer()
        {
            
        }
        private Customer
        (   int customerId,
            string firstName,
            string lastName,
            string emailAddress,
            DateOnly dateOfBirth,
            List<Address> addresses
        )
        {
            CustomerId = customerId;
            _firstName = firstName;
            _lastName = lastName;
            _emailAddress = emailAddress;
            _dateOfBirth = dateOfBirth;
            _addresses = addresses;

            Validate
            (
                    firstName: firstName,
                    lastName: lastName,
                    dateOfBirth: dateOfBirth.ToDateTime(TimeOnly.MinValue)
            );
        }

        //public methods
        public static Customer RegisterNew
        (
            string firstName,
            string lastName,
            string emailAddress,
            DateTime dateOfBirth,
            List<AddressRequestDTO> addresses
        )
        {
            Customer customer = new Customer();
            customer.SetFirstName(firstName: firstName);
            customer.SetLastName(lastName: lastName);
            customer.SetEmailAddress(emailAddress: emailAddress);
            customer.SetDateOfBirth(dateOfBirth: dateOfBirth);
            customer.SetAddresses(addresses: addresses);
            customer.Validate(firstName: firstName, lastName: lastName, dateOfBirth: dateOfBirth);

            return customer;
        }

        public static Customer SetExistingInfo
        (
            int customerId,
            string firstName,
            string lastName,
            string emailAddress,
            DateOnly dateOfBirth,
            List<Address> addresses
        )
        {
            Customer customer = new Customer
            (   customerId: customerId,
                firstName: firstName,
                lastName: lastName,
                emailAddress: emailAddress,
                dateOfBirth: dateOfBirth,
                addresses: addresses
            );
            customer.SetCustomerId(customerId: customerId);
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
                _isValid = false;
                ErrorMessageIfIsNotValid = DomainResponseMessages.CustomerCustomerIdMustBeGreaterThanZeroError;
            }
            CustomerId = customerId;
        }

        private void SetFirstName(string firstName)
        {
           _firstName = firstName;
        }

        private void SetLastName(string lastName)
        {
            _lastName = lastName;
        }

        private void SetEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
        }

        private void SetDateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = DateOnly.FromDateTime(dateTime: dateOfBirth);
        }

        private void SetAddresses(List<AddressRequestDTO> addresses)
        {
            foreach (var address in addresses)
            {
                Addresses.Add(item: new Address
                {
                    ZipCode = address.ZipCode,
                    Street = address.Street,
                    Number = address.Number,
                    Neighborhood = address.Neighborhood,
                    City = address.City,
                    State = address.State,
                    Country = address.Country
                });
            }
        }

        private void Validate(string firstName, string lastName, DateTime dateOfBirth)
        {   
             if (firstName.Length > CustomerConstantsRules.MAXIMUM_CHARACTERS_FIRST_NAME || lastName.Length > CustomerConstantsRules.MAXIMUM_CHARACTERS_LAST_NAME)
            {
                _isValid = false;
                ErrorMessageIfIsNotValid = DomainResponseMessages.MaximumOf40CharactersError;
            }

            DateTime dateNow = DateTime.Now;

            if (dateOfBirth.ToUniversalTime().Date > dateNow.Date)
            {
                _isValid = false;
                ErrorMessageIfIsNotValid = DomainResponseMessages.DateOfBirthError;
            }

            if (Addresses.Count() == 0)
            {
                _isValid = false;
                ErrorMessageIfIsNotValid = DomainResponseMessages.MustHaveAtLeastOneAddress;
            }

            if (ErrorMessageIfIsNotValid == string.Empty)
            {
                _isValid = true;
            }
        }
    }
}