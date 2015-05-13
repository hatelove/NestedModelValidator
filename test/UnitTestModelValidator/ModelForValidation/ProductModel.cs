using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class ProductModel
    {
        [NestedValidation]
        public Person MyPerson { get; set; }

        [NestedValidation]
        public Product MyProduct { get; set; }
    }
}