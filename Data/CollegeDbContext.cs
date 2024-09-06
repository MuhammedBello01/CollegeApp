using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDbContext : DbContext
{
    public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options)
    {
            
    }
    DbSet<Student> Students { get; set; }

    //Override this method to add seed/default data to ur database then run migration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<Student>().HasData(new List<Student>()
        //{
        //    new Student
        //    {
        //        Id = 1,
        //        StudentName = "BabaMoh",
        //        Address = "Lagos",
        //        Email = "Slymmd@gmail.com",
        //        Dob = new DateTime(2022, 12, 12)
        //    },
        //    new Student
        //    {
        //        Id = 2,
        //        StudentName = "Cashoo",
        //        Address = "Ilorin",
        //        Email = "Cashho@gmail.com",
        //        Dob = new DateTime(2023, 10, 13)
        //    }
        //});

        //moved to StudentConfig
        modelBuilder.ApplyConfiguration(new StudentConfig());
      
    }
}