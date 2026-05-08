
using System.ComponentModel.DataAnnotations;

public class Student
{
    [Required(ErrorMessage = "Name is Required!")]
    public string Name { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Email is Required!")]
    public string Email { get; set; }
    [Range(18, 60, ErrorMessage = "Age must be between 18 to 60")]
    public int Age { get; set; }

}