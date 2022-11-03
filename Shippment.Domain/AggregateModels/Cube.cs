using DomainBase;

namespace Shippment.Domain.AggregateModels
{
    public enum UnitOfVolume
    {
        CubicCentimeter,
        CubicDecimeter,
        CubicMeter
    }

    public class Cube : ValueObject
    {
        public Cube(double widthNumber, double heightNumber, double lengthNumber)
        {
            Width = new Line(widthNumber);
            Height = new Line(heightNumber);
            Length = new Line(lengthNumber);
        }

        public Cube(Line width, Line height, Line length)
        {
            Width = width;
            Height = height;
            Length = length;
        }

        public Line Width { get; private set; }
        public Line Height { get; private set; }
        public Line Length { get; private set; }
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
