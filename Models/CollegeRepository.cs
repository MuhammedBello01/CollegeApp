using ColleegeApp.Models;

namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static readonly List<Student> Students = new List<Student>()
        {
            new Student() { StudentName = "Moh", Address = "My house", Email = "Slymmd@gmail.com", Id = 1 },
            new Student() { StudentName = "Cashbook", Address = "Google it", Email = "Cashoo@gmail.com", Id = 2 }
        };
    }
}
