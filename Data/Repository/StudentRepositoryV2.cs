
namespace CollegeApp.Data.Repository
{
    public class StudentRepositoryV2 : CollegeRepository<Student>, IStudentRepositoryV2
    {
        private CollegeDbContext _dbContext { get; set; }
        public StudentRepositoryV2(CollegeDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            //todo: write code to return students fee status
            return null;
        }
    }
}
