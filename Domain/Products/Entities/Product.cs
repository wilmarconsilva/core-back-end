using Domain.Products.Enums;
using Domain.Shared.Entities;
using Domain.Shared.Utilities;

namespace Domain.Products.Entities
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
            ValueValidatorUtility.ValidateValue(description, nameof(Description));

            _description = description;
        }

        private void SetMeasureUnit(MeasureUnit measureUnit)
        {
            ValueValidatorUtility.ValidateEnum(measureUnit, nameof(MeasureUnit));

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
