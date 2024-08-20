using ColleegeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ColleegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllstudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(CollegeRepository.Students);
        }

        [HttpGet]
        [Route("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student?> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students?.FirstOrDefault(s => s?.Id == id);
            if (student == null)
                return NotFound($"Student with id {id} not found");

            return Ok(student);
        }

        [HttpGet]
        [Route("name", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student?> GetStudentById(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest();

            var student = CollegeRepository.Students?.FirstOrDefault(s => string.Equals(s.StudentName, Name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
                return NotFound($"Student with name {Name} not found");

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

            var studentToDelete = CollegeRepository.Students?.Where(s => s?.Id == id).FirstOrDefault();

            if (studentToDelete == null)
                return NotFound($"Student with id {id} not found");

            return Ok(true);
        }
    }
}
