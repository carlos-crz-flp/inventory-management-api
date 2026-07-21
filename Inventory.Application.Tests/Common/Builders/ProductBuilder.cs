using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Tests.Common.Builders
{
    public sealed class ProductBuilder
    {
        private string _sku = TestConstants.ValidSku;
        private string _name = TestConstants.ValidProductName;
        private Guid _categoryId = Guid.NewGuid();
        private int _stock;
        private bool _active = true;

        public ProductBuilder WithSku(string sku)
        {
            _sku = sku;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBuilder WithCategory(Guid categoryId)
        {
            _categoryId = categoryId;
            return this;
        }

        public ProductBuilder WithStock(int stock)
        {
            _stock = stock;
            return this;
        }

        public ProductBuilder Inactive()
        {
            _active = false;
            return this;
        }

        public Product Build()
        {
            var product = new Product(
                Sku.Create(_sku),
                ProductName.Create(_name),
                _categoryId);

            if (_stock > 0)
            {
                product.IncreaseStock(_stock);
            }

            if (!_active)
            {
                product.Deactivate();
            }

            return product;
        }
    }
}