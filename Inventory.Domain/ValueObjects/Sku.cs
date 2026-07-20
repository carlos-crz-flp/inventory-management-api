namespace Inventory.Domain.ValueObjects
{
    public sealed class Sku : IEquatable<Sku>
    {
        public string Value { get; }

        private Sku(string value)
        {
            Value = value;
        }

        public static Sku Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("SKU is required.");

            value = value.Trim().ToUpperInvariant();

            if (value.Length > 50)
                throw new DomainException("SKU cannot exceed 50 characters.");

            return new Sku(value);
        }

        public bool Equals(Sku? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj)
            => Equals(obj as Sku);

        public override int GetHashCode()
            => Value.GetHashCode();

        public override string ToString()
            => Value;
    }
}