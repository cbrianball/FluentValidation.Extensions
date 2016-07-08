using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentValidation.Extensions.DataAnnotations
{
    internal static class DataAnnotationsHelper
    {
        internal static IEnumerable<Results.ValidationFailure> ExecuteValidationAttribute<T>(T instance, ValidationContext<T> ctx, ValidationAttribute attribute)
        {
            var result = attribute.GetValidationResult(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance));

            if (result == ValidationResult.Success) yield break;

            yield return result.ToValidationFailure(instance);
        }
    }
}
