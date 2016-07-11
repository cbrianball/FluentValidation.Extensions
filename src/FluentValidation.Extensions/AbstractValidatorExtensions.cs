using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidation.Extensions
{
    /// <summary>
    /// A set of extension methods for the <see cref="AbstractValidator{T}"/> class.
    /// </summary>
    public static class AbstractValidatorExtensions
    {
        /// <summary>
        /// Scans for properties with complex types. When one is found, a validator is requested;
        /// if one is found, it is included as a child validator.        
        /// </summary>
        /// <typeparam name="T">Type type to be validated</typeparam>
        /// <param name="source">The validator class</param>
        /// <param name="factory">The <see cref="IValidatorFactory"/> instance used to locate child validator types; <see cref="ValidatorFactories.Default"/> will be used if one is not provided</param>
        /// <param name="ruleSet">The rule set assigned to the discovered validators (default: null)</param>
        public static void SetChildValidators<T>(this AbstractValidator<T> source, IValidatorFactory factory = null, string ruleSet = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            factory = factory ?? ValidatorFactories.Default;
            if (factory == null)
                throw new InvalidOperationException($"Parameter '{nameof(factory)}' is null and no default was found at {typeof(ValidatorFactories).FullName}.{nameof(ValidatorFactories.Default)}");

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
