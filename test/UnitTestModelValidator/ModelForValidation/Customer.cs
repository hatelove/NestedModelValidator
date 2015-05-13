using System.ComponentModel.DataAnnotations;

namespace UnitTestModelValidator.ModelForValidation
{
    public class Customer
    {
        [Range(0, 2)]
        public int Id { get; set; }

        [Range(18, 80)]
        public int Age { get; set; }
    }
}