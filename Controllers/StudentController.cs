using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        public StudentController( ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("GetAllStudents", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDto>> GetAllStudents()
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
            _logger.LogInformation("GetStudents Method Started");
            var students = CollegeRepository.Students.Select(s => new StudentDto()
            {
                Id = s.Id,
                Email = s.Email,
                Address = s.Address,
                StudentName = s.StudentName
            });

            return Ok(students);
        }

        [HttpGet]
        [Route("GetStudentById/{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto?> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest(); 
            }

            var student = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                _logger.LogError("Student with the given Id not found");
                return NotFound($"Student with id {id} not found");
            }
              

            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Email = student.Email,
                Address = student.Address,
                StudentName = student.StudentName,
            };
            return Ok(studentDto);
        }

        [HttpGet]
        [Route("GetStudentByName/{name:alpha}", Name = "Students/GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto?> GetStudentById(string name)
        {
            _logger.LogInformation("GetStudentByName Method Started");
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }
                

            var student = CollegeRepository.Students.FirstOrDefault(s => string.Equals(s.StudentName, name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
                return NotFound($"Student with name {name} not found");
            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Email = student.Email,
                Address = student.Address,
                StudentName = student.StudentName,
            };
            return Ok(studentDto);
        }

        [HttpDelete]
        [Route("DeleteStudent/{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK,  Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> DeleteStudentById(int id)
        {
            _logger.LogInformation("Delete Method Started");
            if (id <= 0)
            {
                _logger.LogError($"Invalid Deletion Id: {id}");
                return BadRequest();
            }
            var studentToDelete = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);

            if (studentToDelete == null)
            {
                _logger.LogError($"Student with id {id} not found");
                return NotFound($"Student with id {id} not found");
            }
            CollegeRepository.Students.Remove(studentToDelete);
            return Ok($"Student with id {id} was successfully deleted.");
        }

        [HttpPost]
        [Route("CreateStudent", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> CreateStudent([FromBody] StudentDto? model)
        {
            if (model == null) return BadRequest();
            // if (model.AdmissionDate < DateTime.Now)
            // {
            //     ModelState.AddModelError("Admission Date Error",
            //         "Admission date must be greater than or equal to today's date");
            //     return BadRequest(ModelState);
            // }
            //int newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            var newId = (CollegeRepository.Students.LastOrDefault()?.Id ?? 0) + 1;

            Student student = new Student()
            {
                Id = newId,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address
            };
            model.Id = student.Id;
            CollegeRepository.Students.Add(student);
            //return Ok(model);
            //return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            return Ok(new { Message = "Student successfully added", Student = model });

        }

        [HttpPut]
        [Route("Update", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> UpdateStudent([FromBody] StudentDto? model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();

            var theStudentToUpdate = CollegeRepository.Students.FirstOrDefault(s => s.Id == model.Id);
            if (theStudentToUpdate == null)
                return NotFound($"Student with Id {model.Id} not found");
            
            theStudentToUpdate.StudentName = model.StudentName;
            theStudentToUpdate.Address = model.Address;
            theStudentToUpdate.Email = model.Email;
            
            // var response = new UpdateStudentResponse
            // {
            //     Status = "Success",
            //     Message = "Student successfully added",
            //     Data = theStudentToUpdate
            // };
            
            return Ok("Student record updated successfully");
        }
        
        [HttpPatch]
        [Route("{id:int}/UpdatePartial", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDto>? patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();
 
            var theStudentToUpdate = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (theStudentToUpdate == null)
                return NotFound($"Student with Id {id} not found");

            var studentDto = new StudentDto()
            {
                Id = theStudentToUpdate.Id,
                StudentName = theStudentToUpdate.StudentName,
                Email = theStudentToUpdate.Email,
                Address = theStudentToUpdate.Address
            };
            patchDocument.ApplyTo(studentDto, ModelState);
            if (!ModelState.IsValid)
                BadRequest();
            
            theStudentToUpdate.StudentName = studentDto.StudentName;
            theStudentToUpdate.Address = studentDto.Address;
            theStudentToUpdate.Email = studentDto.Email;
             
            return Ok("Student record updated successfully");
        }
    }
}
