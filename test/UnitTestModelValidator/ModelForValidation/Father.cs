using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class Father
    {
        [Range(1, 130)]
        public int Age { get; set; }

        [Required]
        public string Name { get; set; }

        [NestedValidation]
        public IEnumerable<Son> Sons { get; set; }
    }

    public class Son : IValidatableObject
    {
        [Range(1, 130)]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (this.Age % 2 == 0)
            {
                var memberNames = new List<string> { "Age" };
                result.Add(new ValidationResult("Age不得為偶數", memberNames));
            }

            return result;
        }
    }
}