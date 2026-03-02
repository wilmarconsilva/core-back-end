using Domain.Utilities.Entities;
using Domain.ValueObjects.Enums;

namespace Domain.Entities
{
    public class Product : Entity
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Product() { }

        private string _description = null!;
        private MeasureUnit _measureUnit;

        public override string Description { get => _description; protected set => SetDescription(value); }

        public MeasureUnit MeasureUnit { get => _measureUnit; set => SetMeasureUnit(value); }

        private void SetDescription(string description)
        {
            Validator.ValidateValue(description, nameof(Description));

            _description = description;
        }

        private void SetMeasureUnit(MeasureUnit measureUnit)
        {
            Validator.ValidateEnum(measureUnit, nameof(MeasureUnit));

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
