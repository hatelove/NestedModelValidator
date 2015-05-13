using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    public class PersonModel
    {
        [NestedValidation]
        public Person MyPerson { get; set; }

        [NestedValidation]
        public Customer MyCustomer { get; set; }
    }
}