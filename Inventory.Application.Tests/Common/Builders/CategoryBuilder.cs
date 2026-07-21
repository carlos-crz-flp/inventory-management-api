using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Tests.Common.Builders
{
    public sealed class CategoryBuilder
    {
        private string _name = TestConstants.ValidCategoryName;
        private bool _active = true;

        public CategoryBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CategoryBuilder Inactive()
        {
            _active = false;
            return this;
        }

        public Category Build()
        {
            var category = new Category(
                CategoryName.Create(_name));

            if (!_active)
            {
                category.Deactivate();
            }

            return category;
        }
    }
}