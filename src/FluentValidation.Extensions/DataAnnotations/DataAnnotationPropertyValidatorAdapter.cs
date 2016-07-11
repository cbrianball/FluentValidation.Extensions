using FluentValidation.Validators;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation.Extensions.DataAnnotations
{
    /// <summary>
    /// Class to utilize DataAnnotation attributes as validators inside of FluentValidate.
    /// Any attributes that inherit from ValidationAttribute may be used.
    /// </summary>
    public class DataAnnotationPropertyValidatorAdapter : PropertyValidator
    {
        private readonly ValidationAttribute _validationAttribute;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAnnotationPropertyValidatorAdapter"/> class using the provided <see cref="ValidationAttribute"/>.
        /// </summary>
        /// <param name="attribute">The attribute that will perform the validation(s).</param>
        public DataAnnotationPropertyValidatorAdapter(ValidationAttribute attribute) : base(attribute.FormatErrorMessage("{PropertyName}"))
        {
            _validationAttribute = attribute;
        }

        /// <summary>
        /// Performs validation by invoke the <see cref="ValidationAttribute"/> that was passed into the constructor. 
        /// </summary>
        /// <param name="context">The context from FluentValidation of the current validation operation</param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(context.Instance) { MemberName = context.PropertyName };
            
            return _validationAttribute.GetValidationResult(context.PropertyValue, ctx) == ValidationResult.Success;
        }
    }
}
