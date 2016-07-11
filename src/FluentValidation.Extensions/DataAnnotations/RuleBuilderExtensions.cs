using FluentValidation.Extensions.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation.Extensions.DataAnnotations
{
    /// <summary>
    /// Contains extension methods for <see cref="IRuleBuilder{T, TProperty}"/> 
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Associates a <see cref="ValidationAttribute"/> with the property being configured. 
        /// </summary>
        /// <typeparam name="T">The type to be validated</typeparam>
        /// <typeparam name="TProperty">The type of the property to be validated</typeparam>
        /// <param name="source">The current <see cref="IRuleBuilder{T, TProperty}"/> </param>
        /// <param name="attribute">The attribute to perform the validation</param>
        /// <returns>Returns <see cref="IRuleBuilderOptions{T, TProperty}"/> to continue the method chaining.</returns>
        public static IRuleBuilderOptions<T, TProperty> UseDataAnnotation<T, TProperty>(this IRuleBuilder<T, TProperty> source, ValidationAttribute attribute)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SetValidator(new DataAnnotationPropertyValidatorAdapter(attribute));
        }
    }
}
