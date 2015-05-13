using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infra.ModelValidator
{
    /// <summary>
    /// 一個ValidationAttribute只能回傳一個ValidationResult, 透過CompositeValidationResult的擴充，可從Results獲得多個ValidationResult的集合。
    /// 透過AddResult()也可以加入ValidationResult的element。
    /// </summary>
    public class CompositeValidationResult : ValidationResult
    {
        /// <summary>
        /// The _results
        /// </summary>
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public CompositeValidationResult(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return this._results;
            }
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <param name="displayName">The display name.</param>
        public void AddResult(ValidationResult validationResult, string displayName)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult");
            }

            var fieldName = validationResult.MemberNames.FirstOrDefault();
            if (fieldName != null)
            {
                var propertyName = string.Format("{0}.{1}", displayName, fieldName);
                var errorMessage = validationResult.ErrorMessage.Replace(fieldName, propertyName);

                var memberNames = validationResult.MemberNames.Select(x => propertyName).ToList();
                var result = new ValidationResult(errorMessage, memberNames);

                this._results.Add(result);
            }
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="index">The index.</param>
        public void AddResult(ValidationResult validationResult, string displayName, int index)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult");
            }

            var fieldName = validationResult.MemberNames.FirstOrDefault();
            if (fieldName != null)
            {
                var propertyName = string.Format("{0}[{2}].{1}", displayName, fieldName, index.ToString());
                var errorMessage = validationResult.ErrorMessage.Replace(fieldName, propertyName);

                var memberNames = validationResult.MemberNames.Select(x => propertyName).ToList();
                var result = new ValidationResult(errorMessage, memberNames);

                this._results.Add(result);
            }
        }
    }
}