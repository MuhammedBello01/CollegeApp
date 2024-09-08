using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDbContext _dbContext;
        public StudentController( ILogger<StudentController> logger, CollegeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetAllStudents", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            _logger.LogInformation("GetStudents Method Started");
            var students = await _dbContext.Students.Select(s => new StudentDto()
            {
                Id = s.Id,
                Email = s.Email,
                Address = s.Address,
                StudentName = s.StudentName,
                Dob = s.Dob,
            }).ToListAsync();
            return Ok(students);
        }

        [HttpGet]
        [Route("GetStudentById/{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto?>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest(); 
            }

            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
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
        public async Task<ActionResult<StudentDto?>> GetStudentByNameAsync(string name)
        {
            _logger.LogInformation("GetStudentByName Method Started");
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }
                
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.StudentName.ToLower() == name.ToLower());
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
        public async Task<ActionResult<string>> DeleteStudentByIdAsync(int id)
        {
            _logger.LogInformation("Delete Method Started");
            if (id <= 0)
            {
                _logger.LogError($"Invalid Deletion Id: {id}");
                return BadRequest();
            }
            var studentToDelete = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (studentToDelete == null)
            {
                _logger.LogError($"Student with id {id} not found");
                return NotFound($"Student with id {id} not found");
            }
            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
            return Ok($"Student with id {id} was successfully deleted.");
        }

        [HttpPost]
        [Route("CreateStudent", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> CreateStudentAsync([FromBody] StudentDto? model)
        {
            if (model == null) return BadRequest();
            // if (model.AdmissionDate < DateTime.Now)
            // {
            //     ModelState.AddModelError("Admission Date Error",
            //         "Admission date must be greater than or equal to today's date");
            //     return BadRequest(ModelState);
            // }
            //var newId = (_dbContext.Students.LastOrDefault()?.Id ?? 0) + 1;

            Student student = new Student()
            {
                // Id = newId,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address,
                Dob = model.Dob
            };
            model.Id = student.Id;
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            //return Ok(model);
            //return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            return Ok(new { Message = "Student successfully added", Student = model });

        }

        [HttpPut]
        [Route("Update", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateStudentAsync([FromBody] StudentDto? model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();

            var theStudentToUpdate = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.Id);
            if (theStudentToUpdate == null)
                return NotFound($"Student with Id {model.Id} not found");
            
            var newRecord = new Student
            {
                Id = theStudentToUpdate.Id,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email,
                Dob = model.Dob,
            };
            _dbContext.Students.Update(newRecord);

            // var theStudentToUpdate = _dbContext.Students.FirstOrDefault(s => s.Id == model.Id);
            //theStudentToUpdate.StudentName = model.StudentName;
            //theStudentToUpdate.Address = model.Address;
            //theStudentToUpdate.Email = model.Email;
            //theStudentToUpdate.Dob = model.Dob;
            await _dbContext.SaveChangesAsync();
            return Ok("Student record updated successfully");
        }
        
        [HttpPatch]
        [Route("{id:int}/UpdatePartial", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDto>? patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();
 
            var theStudentToUpdate = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
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
                BadRequest(ModelState);
            
            theStudentToUpdate.StudentName = studentDto.StudentName;
            theStudentToUpdate.Address = studentDto.Address;
            theStudentToUpdate.Email = studentDto.Email;

            await _dbContext.SaveChangesAsync();
            return Ok("Student record updated successfully");
        }
    }
}
