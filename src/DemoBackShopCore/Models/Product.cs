using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Models
{
    public class Product
    {
        //Private Properties
        private int _productId;
        private string _code;
        private string _name;


        //Public Properties
        public int ProductId { get; private set; }
        public string Code => _code;
        public string Name => _name;
        public bool IsValid { get; private set; } = false;
        public string ErrorMessageIfIsNotValid { get; private set; } = string.Empty;

        //Private Constructors
        private Product()
        {
            
        }
        private Product
        (
            int productId,
            string code,
            string name
        )
        {
            _productId = productId;
            ProductId = productId;
            _code = code;
            _name = name;

            Validate();
        }


        //Public Methods
        public static Product RegisterNew(string code, string name)
        {
            Product product = new Product();

            product.SetCode(code: code);
            product.SetName(name: name);
            product.Validate();

            return product;
        }

        public static Product SetExistingInfo(int productId, string code, string name)
        {
            Product product = new Product(productId: productId, code: code, name: name);

            return product;
        }

        public void Validate()
        {
            if (_productId < 0)
            {
                ErrorMessageIfIsNotValid = DomainResponseMessages.ProductIdMustBeGreaterThanZeroError;
                IsValid = false;
            }
            if (_code.Length > 40)
            {
                ErrorMessageIfIsNotValid = $"Code field {DomainResponseMessages.MaximumOf40CharactersError}";
                IsValid = false;
            }
            if (_name.Length > 40)
            {
                ErrorMessageIfIsNotValid = $"Name field {DomainResponseMessages.MaximumOf40CharactersError}";
                IsValid = false;
            }

            IsValid = true;
        }

        //Private Methods
        public void SetId(int productId)
        {
            _productId = productId;
        }
        public void SetCode(string code)
        {
            _code = code;
        }
        public void SetName(string name)
        {
            _name = name;
        }
    }
}