using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class GrandFather
    {
        [StringLength(5, MinimumLength = 3)]
        public string Name { get; set; }

        [NestedValidation]
        public List<Father> Fathers { get; set; }
    }
}