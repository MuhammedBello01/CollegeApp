using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>() 
            {
                 new Student
                 {
                    Id = 1,
                    StudentName = "BabaMoh",
                    Address = "Lagos",
                    Email = "Slymmd@gmail.com",
                    Dob = new DateTime(2022, 12, 12)
                 },
                 new Student
                 {
                    Id = 2,
                    StudentName = "Cashoo",
                    Address = "Ilorin",
                    Email = "Cashho@gmail.com",
                    Dob = new DateTime(2023, 10, 13)
                 }
            });

            builder.HasOne(n => n.Department)
                .WithMany(n => n.Students)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_Students_Department");
        }
    }
}
