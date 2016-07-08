using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidation.Extensions
{
    public static class AbstractValidatorExtensions
    {
        public static void SetChildValidators<T>(this AbstractValidator<T> source, string ruleSet = null)
        {
            source.SetChildValidators(ValidatorFactories.Default, ruleSet);
        }

        public static void SetChildValidators<T>(this AbstractValidator<T> source, IValidatorFactory factory, string ruleSet = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (factory == null) throw new ArgumentNullException(nameof(source));

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var type = property.PropertyType;

                if (type.IsValueType) continue;

                IValidator validator = null;
                PropertyRule rule = null;

                var elementType = type.GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0])
                    .FirstOrDefault();

                if (elementType != null && elementType.IsValueType == false)
                {
                    validator = factory.GetValidator(elementType);
                    if (validator == null) continue;

                    rule = Helper.CreateCollectionPropertyRule(property, elementType, source.CascadeMode);
                }
                else
                {
                    validator = factory.GetValidator(type);
                    if (validator == null) continue;

                    rule = Helper.CreatePropertyRule(property, source.CascadeMode);
                }

                rule.RuleSet = ruleSet;
                rule.AddValidator(new ChildValidatorAdaptor(validator));

                source.AddRule(rule);
            }
        }
    }
}
