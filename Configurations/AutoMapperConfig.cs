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
            //CreateMap<StudentDto, Student>().ReverseMap();

            //to set a message that applies to all properties in the model that has no value
           // CreateMap<StudentDto, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "Nil" : n);

            //to set a default message for property that has no value
            CreateMap<StudentDto, Student>().ReverseMap()
                .ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No Address Found" : n.Address));

        }
    }
}
