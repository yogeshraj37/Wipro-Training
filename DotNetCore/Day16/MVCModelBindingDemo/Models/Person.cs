namespace MVCModelBindingDemo.Models
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; } = new();
    }
}