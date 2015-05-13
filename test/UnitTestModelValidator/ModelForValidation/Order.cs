using System.ComponentModel.DataAnnotations;
using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class Order
    {
        [Range(0, 100)]
        public int Id { get; set; }

        [NestedValidation]
        public Customer MyCustomer { get; set; }
    }
}