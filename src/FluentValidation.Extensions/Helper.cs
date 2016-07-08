using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentValidation.Extensions
{
    internal static class Helper
    {
        internal static Results.ValidationFailure ToValidationFailure(this ValidationResult source, object value)
        {
            if (source == ValidationResult.Success) return null;

            return new Results.ValidationFailure(source.MemberNames.FirstOrDefault(), source.ErrorMessage, value);
        }

        internal static IEnumerable<Results.ValidationFailure> ToValidationFailures(this IEnumerable<ValidationResult> source, object value)
        {
            if (source == null || source.Any() == false) return Enumerable.Empty<Results.ValidationFailure>();

            return source.Where(v => v != ValidationResult.Success).Select(v => v.ToValidationFailure(value));
        }

        internal static System.ComponentModel.DataAnnotations.ValidationContext ToValidationContext<T>(this ValidationContext<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new System.ComponentModel.DataAnnotations.ValidationContext(source.InstanceToValidate);
        }

        internal static PropertyRule CreatePropertyRule(PropertyInfo property, CascadeMode cascadeMode)
        {
            var expression = CreatePropertyAccessorExpression(property);
            var compiled = expression.Compile() as Func<object, object>;

            return new PropertyRule(property, compiled, expression, () => cascadeMode, property.PropertyType, property.DeclaringType);
        }

        internal static PropertyRule CreateCollectionPropertyRule(PropertyInfo property, Type collectionType, CascadeMode cascadeMode)
        {
            var expression = CreatePropertyAccessorExpression(property);
            var compiled = expression.Compile() as Func<object, object>;

            var collectionRuleType = typeof(CollectionPropertyRule<>).MakeGenericType(collectionType);

            return
                Activator.CreateInstance(collectionRuleType, new object[] { property, compiled, expression, (Func<CascadeMode>)(() => cascadeMode), property.PropertyType, property.DeclaringType })
                as PropertyRule;
        }

        private static LambdaExpression CreatePropertyAccessorExpression(PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(object));
            var boxedParameter = Expression.Convert(parameter, property.DeclaringType);
            var accessor = Expression.Property(boxedParameter, property.Name);
            var boxedReturn = Expression.Convert(accessor, typeof(object));

            return Expression.Lambda(boxedReturn, parameter);
        }
    }
}
