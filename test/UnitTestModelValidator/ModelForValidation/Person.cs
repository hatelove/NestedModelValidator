using System;
using System.ComponentModel.DataAnnotations;

namespace UnitTestModelValidator.ModelForValidation
{
    public class Person
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }
}