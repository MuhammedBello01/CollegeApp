namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id, bool useNoTracking = false);
        Task<Student> GetStudentByNameAsync(string name);
        Task<int> CreateStudentAsync(Student student);
        Task<int> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(Student studentToDelete);


    }
}
