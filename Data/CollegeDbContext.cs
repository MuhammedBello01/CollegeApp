using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDbContext : DbContext
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options)
    {
            
    }
    public DbSet<Student> Students { get; set; }

    public DbSet<Department> Departments { get; set; }

    //Override this method to add seed/default data to ur database then run migration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.ApplyConfiguration(new StudentConfig());
        modelBuilder.ApplyConfiguration(new DepartmentConfig());

    }
}