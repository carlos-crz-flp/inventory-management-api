namespace Inventory.Domain.ValueObjects
{
    public sealed class Quantity : IEquatable<Quantity>
    {
        public int Value { get; }

        private Quantity(int value)
        {
            Value = value;
        }

        public static Quantity Create(int value)
        {
            if (value < 0)
                throw new DomainException("Quantity cannot be negative.");

            return new Quantity(value);
        }

        public Quantity Add(int amount)
            => Create(Value + amount);

        public Quantity Subtract(int amount)
        {
            if (amount > Value)
                throw new DomainException("Insufficient stock.");

            return Create(Value - amount);
        }

        public bool Equals(Quantity? other)
            => other is not null && Value == other.Value;

        public override bool Equals(object? obj)
            => Equals(obj as Quantity);

        public override int GetHashCode()
            => Value.GetHashCode();

        public override string ToString()
            => Value.ToString();
    }
}