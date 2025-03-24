using System.ComponentModel.DataAnnotations;
using DemoBackShopCore.ConstantsModelsRules;
using DemoBackShopCore.Models;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.DTOs
{
    public class CustomerRequestDTO
    {
        [Required]
        [MaxLength(length: CustomerConstantsRules.MAXIMUM_CHARACTERS_FIRST_NAME , ErrorMessage = DomainResponseMessages.MaximumOf40CharactersError)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(length: CustomerConstantsRules.MAXIMUM_CHARACTERS_LAST_NAME, ErrorMessage = DomainResponseMessages.MaximumOf40CharactersError)]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = DomainResponseMessages.EmailFieldIsNotAValid)]
        public string EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
    }
}