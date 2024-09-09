using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Student, StudentDto>();
            //CreateMap<StudentDto, Student>();

            CreateMap<StudentDto, Student>().ReverseMap();
        }
    }
}
