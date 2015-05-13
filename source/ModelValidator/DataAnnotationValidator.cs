using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infra.ModelValidator
{
    /// <summary>
    /// model validator，透過DataAnnotation ValidationAttribute來標記 Validation rules
    /// </summary>
    public class DataAnnotationValidator
    {
        /// <summary>
        /// Gets the validation results.
        /// </summary>
        /// <value>
        /// The validation results.
        /// </value>
        public List<ValidationResult> ValidationResults
        {
            get;
            private set;
        }

        /// <summary>
        /// try to validate model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>isValid</returns>
        public bool TryValidate(object model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var result = Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);

            this.ValidationResults = new List<ValidationResult>();

            this.ValidationResults.AddRange(validationResults.OfType<ValidationResult>().Where(x => !(x is CompositeValidationResult)));

            var customValidationResults = validationResults.OfType<CompositeValidationResult>();

            foreach (var customValidationResult in customValidationResults)
            {
                this.ValidationResults.AddRange(customValidationResult.Results);
            }

            return result;
        }
    }
}