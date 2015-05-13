﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infra.ModelValidator
{
    /// <summary>
    /// 用來標記組合式的model，或是集合型態需要展開驗證的property。
    /// </summary>
    public class NestedValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified entity with respect to the current validation attribute.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object entity, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            if (entity == null)
            {
                return ValidationResult.Success;
            }

            var displayName = validationContext.DisplayName;
            var compositeResults = new CompositeValidationResult(string.Format("Validation for {0} failed!", displayName));

            IEnumerable items = entity as IEnumerable;

            if (items != null)
            {
                var index = 0;
                foreach (var item in items)
                {
                    var validator = new DataAnnotationValidator();

                    validator.TryValidate(item);
                    var results = validator.ValidationResults;

                    if (results.Count != 0)
                    {
                        results.ForEach(x => compositeResults.AddResult(x, displayName, index));
                    }

                    index++;
                }
                
                var isAnythingInvalid = compositeResults.Results.Any();

                return isAnythingInvalid ? compositeResults : ValidationResult.Success;
            }
            else
            {
                var validator = new DataAnnotationValidator();

                validator.TryValidate(entity);
                var results = validator.ValidationResults;

                if (results.Count != 0)
                {
                    results.ForEach(x => compositeResults.AddResult(x, displayName));
                    return compositeResults;
                }
            }

            return ValidationResult.Success;
        }
    }
}