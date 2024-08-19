namespace ColleegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students = new List<Student>()
        {
            new Student() { StudentName = "Moh", Address = "My house", Email = "Slymmd@gmail.com", Id = 1 },
            new Student() { StudentName = "Cashhoo", Address = "Google it", Email = "Cashoo@gmail.com", Id = 2 }
        };
    }
}
