using ColleegeApp.Models;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            //var students = new List<StudentDTO>();
            //foreach (var item in CollegeRepository.Students)
            //{
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id = item.Id,
            //        Email = item.Email,
            //        Address = item.Address,
            //        StudentName = item.StudentName
            //    };
            //    students.Add(obj);
            //}

            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                Email = s.Email,
                Address = s.Address,
                StudentName = s.StudentName
            });

            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO?> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound($"Student with id {id} not found");

            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                Email = student.Email,
                Address = student.Address,
                StudentName = student.StudentName,
            };
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "Students/GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO?> GetStudentById(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = CollegeRepository.Students.FirstOrDefault(s => string.Equals(s.StudentName, name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
                return NotFound($"Student with name {name} not found");
            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                Email = student.Email,
                Address = student.Address,
                StudentName = student.StudentName,
            };
            return Ok(student);
        }

        [HttpDelete]
        [Route("{Id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudentById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var studentToDelete = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);

            if (studentToDelete == null)
                return NotFound($"Student with id {id} not found");

            return Ok(true);
        }
    }
}
