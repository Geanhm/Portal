using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Tests.Helpers
{
    internal static class ValidationHelper
    {
        public static IList<ValidationResult> Validate(object instance)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(instance);
            Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
            return results;
        }
    }
}