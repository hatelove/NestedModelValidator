using System.ComponentModel.DataAnnotations;
using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class OrderGroup
    {
        [Required(AllowEmptyStrings = false)]
        public string OrderName { get; set; }

        [NestedValidation]
        public Order MyOrder { get; set; }
    }
}