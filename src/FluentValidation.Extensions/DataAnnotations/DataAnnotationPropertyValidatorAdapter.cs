using FluentValidation.Validators;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation.Extensions.DataAnnotations
{
    public class DataAnnotationPropertyValidatorAdapter : PropertyValidator
    {
        private readonly ValidationAttribute _validationAttribute;
        public DataAnnotationPropertyValidatorAdapter(ValidationAttribute attribute) : base(attribute.FormatErrorMessage("{PropertyName}"))
        {
            _validationAttribute = attribute;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(context.Instance) { MemberName = context.PropertyName };

            return _validationAttribute.GetValidationResult(context.PropertyValue, ctx) == ValidationResult.Success;
        }
    }
}
