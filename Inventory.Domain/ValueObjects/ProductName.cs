namespace Inventory.Domain.ValueObjects
{
    public sealed class ProductName : IEquatable<ProductName>
    {
        public string Value { get; }

        private ProductName(string value)
        {
            Value = value;
        }

        public static ProductName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Product name is required.");

            value = value.Trim();

            if (value.Length > 150)
                throw new DomainException("Product name cannot exceed 150 characters.");

            return new ProductName(value);
        }

        public bool Equals(ProductName? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj)
            => Equals(obj as ProductName);

        public override int GetHashCode()
            => Value.GetHashCode();

        public override string ToString()
            => Value;
    }
}