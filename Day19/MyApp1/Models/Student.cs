

using System.ComponentModel.DataAnnotations;

public class Students
{
    [Required(ErrorMessage = "Id is Required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is Required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; set; }
     
     [Range (18,60, ErrorMessage ="Age should be between 18 to 60 ")]
    public int age { get; set; }
}