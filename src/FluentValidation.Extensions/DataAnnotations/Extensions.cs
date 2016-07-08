using FluentValidation.Internal;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FluentValidation.Extensions.DataAnnotations
{
    public static class Extensions
    {
        public static void Custom<T>(this AbstractValidator<T> source, ValidationAttribute attribute)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            source.Custom((t, ctx) => DataAnnotationsHelper.ExecuteValidationAttribute(t, ctx, attribute).FirstOrDefault());
        }

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
