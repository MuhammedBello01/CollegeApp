using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data;

public class CollegeDbContext : DbContext
{ 
    DbSet<Student> Students { get; set; }
}