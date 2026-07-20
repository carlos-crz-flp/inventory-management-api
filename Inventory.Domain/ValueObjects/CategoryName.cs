namespace Inventory.Domain.ValueObjects
{
    public sealed class CategoryName : IEquatable<CategoryName>
    {
        public string Value { get; }

        private CategoryName(string value)
        {
            Value = value;
        }

        public static CategoryName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Category name is required.");

            value = value.Trim();

            if (value.Length > 100)
                throw new DomainException("Category name cannot exceed 100 characters.");

            return new CategoryName(value);
        }

        public bool Equals(CategoryName? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj)
            => Equals(obj as CategoryName);

        public override int GetHashCode()
            => Value.GetHashCode();

        public override string ToString()
            => Value;
    }
}