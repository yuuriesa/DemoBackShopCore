using System.ComponentModel.DataAnnotations;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.DTOs
{
    public class ProductRequestDTO
    {
        [Required(ErrorMessage = DomainResponseMessages.CodeIsRequired)]
        [MaxLength(length: 40, ErrorMessage = DomainResponseMessages.MaximumOf40CharactersError)]
        public string Code { get; set; }
        [Required(ErrorMessage = DomainResponseMessages.ProductNameIsRequired)]
        [MaxLength(length: 40, ErrorMessage = DomainResponseMessages.MaximumOf40CharactersError)]
        public string Name { get; set; }
    }
}