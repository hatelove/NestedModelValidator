using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTestModelValidator.ModelForValidation
{
    public class Product : IValidatableObject
    {
        [Range(100, int.MaxValue)]
        public int Cost { get; set; }

        [Range(100, int.MaxValue)]
        public int SellPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var benefitRate = this.SellPrice / Convert.ToDouble(this.Cost);

            var minimumBenefitRate = 0.06;
            if (benefitRate - 1 < minimumBenefitRate)
            {
                var memberNames = new List<string> { "毛利率" };
                results.Add(new ValidationResult(string.Format("毛利率需大於{0}%", minimumBenefitRate * 100), memberNames));
            }

            return results;
        }
    }
}