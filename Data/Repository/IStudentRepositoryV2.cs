namespace CollegeApp.Data.Repository
{
    public interface IStudentRepositoryV2 : ICollegeRepository<Student>
    {
        Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);

    }
}
