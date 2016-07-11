using FluentValidation.Internal;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FluentValidation.Extensions.DataAnnotations
{
    /// <summary>
    /// A set of extension methods for the <see cref="AbstractValidator{T}"/> class used to incorporated Data Annoation attributes into FluentValidation.
    /// </summary>
    public static class AbstractValidatorExtensions
    {
        /// <summary>
        /// Associates a <see cref="ValidationAttribute"/> to be used when validating the type <typeparamref name="T"/>  
        /// </summary>
        /// <typeparam name="T">The type to be validated</typeparam>
        /// <param name="source">The validator class</param>
        /// <param name="attribute">The <see cref="ValidationAttribute"/> to be executed whenever validation is performed</param>
        public static void Custom<T>(this AbstractValidator<T> source, ValidationAttribute attribute)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var usage = attribute.GetType().GetCustomAttributes(typeof(AttributeUsageAttribute), false).Cast<AttributeUsageAttribute>().FirstOrDefault();
            if (usage.ValidOn.HasFlag(AttributeTargets.Class) == false)
                throw new InvalidOperationException("Cannot call Custom with an attribute that is not meant to decorate a class");

            source.Custom((t, ctx) => DataAnnotationsHelper.ExecuteValidationAttribute(t, ctx, attribute).FirstOrDefault());
        }

        /// <summary>
        /// Scans all properpties of the type to be validated and associates the <see cref="ValidationAttribute"/> with the property it is decorating. 
        /// </summary>
        /// <typeparam name="T">The type to be validated</typeparam>
        /// <param name="source">The validator class</param>
        /// <param name="ruleSet">The rule set assigned to the discovered validators (deafult: null)</param>
        public static void SetDataAnnotationValidators<T>(this AbstractValidator<T> source, string ruleSet = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var properties = typeof(T).GetProperties();

            //add property validators
            foreach (var property in properties)
            {
                var validationAttributes = property.GetCustomAttributes(false).OfType<ValidationAttribute>().ToArray();

                if (validationAttributes.Any() == false) continue;

                var rule = Helper.CreatePropertyRule(property, source.CascadeMode);
                rule.RuleSet = ruleSet;

                foreach (var attribute in validationAttributes)
                    rule.AddValidator(new DataAnnotationPropertyValidatorAdapter(attribute));


                source.AddRule(rule);
            }

            //add type validators
            foreach (var validationAttribute in TypeDescriptor.GetAttributes(typeof(T)).OfType<ValidationAttribute>())
            {
                var rule = new DelegateValidator<T>((t, ctx) => DataAnnotationsHelper.ExecuteValidationAttribute(t, ctx, validationAttribute))
                {
                    RuleSet = ruleSet
                };

                source.AddRule(rule);
            }

            //add IValidableObject validation
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(T)))
            {
                var rule = new DelegateValidator<T>((t, ctx) => ((IValidatableObject)t).Validate(ctx.ToValidationContext()).ToValidationFailures(t))
                {
                    RuleSet = ruleSet
                };

                source.AddRule(rule);
            }
        }
    }
}
