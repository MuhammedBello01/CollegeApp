using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllStudentsAsync();
        Task<T> GetStudentByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> GetStudentByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> CreateStudentAsync(T dbRecord);
        Task<T> UpdateStudentAsync(T dbRecord);
        Task<bool> DeleteStudentAsync(T dbRecord);

    }
}
