
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository 
    {
        private CollegeDbContext _dbContext { get; set; }
        public StudentRepository(CollegeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateStudentAsync(Student student)
        {
           _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(Student studentToDelete)
        {
            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id, bool useNoTracking = false)
        {
            if (!useNoTracking)
                return await _dbContext.Students?.FirstOrDefaultAsync(student => student.Id == id);
            else 
                return await _dbContext.Students?.AsNoTracking().FirstOrDefaultAsync(student => student.Id == id);
        }

        public async Task<Student> GetStudentByNameAsync(string name)
        {
            return await _dbContext.Students?.FirstOrDefaultAsync(student => student.StudentName.ToLower() == name);
        }

        public async Task<int> UpdateStudentAsync(Student student)
        {
            _dbContext.Students.Update(student);    
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
