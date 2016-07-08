using FluentValidation.Extensions.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> UseDataAnnotation<T, TProperty>(this IRuleBuilder<T, TProperty> source, ValidationAttribute attribute)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SetValidator(new DataAnnotationPropertyValidatorAdapter(attribute));
        }
    }
}
