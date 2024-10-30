
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        public CollegeDbContext _dbContext { get; set; }
        private DbSet<T> _dbSet;
        public CollegeRepository(CollegeDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAllStudentsAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetStudentByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (!useNoTracking)
                return await _dbSet.FirstOrDefaultAsync(filter);
            else
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<T> GetStudentByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<T> CreateStudentAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<T> UpdateStudentAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteStudentAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
