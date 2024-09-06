using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp.Data;

public class Student
{
    //[Key] We removed this bcos we have created a StudentConfig class and the key is been set there
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? StudentName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime Dob { get; set; }
} 