using Domain.ValueObjects.Enums;

namespace Domain.Entities
{
    public class Product : Entity
    {
        private string _description = null!;
        private MeasureUnit _measureUnit;

        public override string Description { get => _description; protected set => SetDescription(value); }

        public MeasureUnit MeasureUnit { get; }

        public void SetDescription(string description)
        {
            ValidateValue(description, nameof(Description));

            _description = description;
        }

        public void SetMeasureUnit(MeasureUnit measureUnit)
        {
            ValidateEnum(measureUnit, nameof(MeasureUnit));

            _measureUnit = measureUnit;
        }

        public void Fill(string description, MeasureUnit measureUnit)
        {
            SetDescription(description);
            SetMeasureUnit(measureUnit);
        }

        public Product(string description, MeasureUnit measureUnit)
        {
            Fill(description, measureUnit);
        }
    }
}
