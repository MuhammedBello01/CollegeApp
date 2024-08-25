using System.ComponentModel;
using CollegeApp.Models;

namespace CollegeApp.Models;

public class UpdateStudentResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public Student? Data { get; set; }
    public DateTime TimeGenerated { get; set; } = DateTime.Now;
}


