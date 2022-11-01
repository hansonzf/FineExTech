using DomainBase;

namespace Shippment.Domain.AggregateModels
{
    public enum UnitOfVolume
    {
        CubicCentimeter,
        CubicDecimeter,
        CubicMeter
    }

    public class Dimension : ValueObject
    {
        public Dimension(double widthNumber, double heightNumber, double lengthNumber)
        {
            Width = new Length(widthNumber);
            Height = new Length(heightNumber);
            Length = new Length(lengthNumber);
        }

        public Dimension(Length width, Length height, Length length)
        {
            Width = width;
            Height = height;
            Length = length;
        }

        public Length Width { get; private set; }
        public Length Height { get; private set; }
        public Length Length { get; private set; }
        public UnitOfVolume Unit { get; private set; }
        public double Volume
        {
            get
            {
                if (!IsValid())
                    return 0f;

                Length = Length.ChangeUnit(UnitOfLength.Meter);
                Width = Width.ChangeUnit(UnitOfLength.Meter);
                Height = Height.ChangeUnit(UnitOfLength.Meter);

                return Length.Number * Width.Number * Height.Number;
            }
        }

        private bool IsValid()
        {
            return Width is not null &&
                Height is not null &&
                Length is not null;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Length;
            yield return Width;
            yield return Height;
            yield return Unit;
        }
    }
}
