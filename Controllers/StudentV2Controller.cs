using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentV2Controller : ControllerBase
    {
        private readonly ILogger<StudentV2Controller> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;
        public StudentV2Controller(ILogger<StudentV2Controller> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }


        [HttpGet]
        [Route("GetAllStudents", Name = "GetAllStudentsV2")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            _logger.LogInformation("GetStudents Method Started");

            var students = await _studentRepository.GetAllStudentsAsync();
            var studentDtoData = _mapper.Map<List<StudentDto>>(students);
            return Ok(studentDtoData);
        }

        [HttpGet]
        [Route("GetStudentById/{id:int}", Name = "GetStudentByIdV2")]
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

            var student = await _studentRepository.GetStudentByIdAsync(student => student.Id == id);
            if (student == null)
            {
                _logger.LogError("Student with the given Id not found");
                return NotFound($"Student with id {id} not found");
            }


            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpGet]
        [Route("GetStudentByName/{name:alpha}", Name = "Students/GetStudentByNameV2")]
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

            var student = await _studentRepository.GetStudentByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
            if (student == null)
                return NotFound($"Student with name {name} not found");

            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpDelete]
        [Route("DeleteStudent/{id:int}", Name = "DeleteStudentByIdV2")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
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
            var studentToDelete = await _studentRepository.GetStudentByIdAsync(student => student.Id == id);

            if (studentToDelete == null)
            {
                _logger.LogError($"Student with id {id} not found");
                return NotFound($"Student with id {id} not found");
            }
            await _studentRepository.DeleteStudentAsync(studentToDelete);
            return Ok($"Student with id {id} was successfully deleted.");
        }

        [HttpPost]
        [Route("CreateStudent", Name = "CreateStudentV2")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> CreateStudentAsync([FromBody] StudentDto? model)
        {
            if (model == null) return BadRequest();
           
            Student student = _mapper.Map<Student>(model);
            var studentAfterCreation = await _studentRepository.CreateStudentAsync(student);
            model.Id = studentAfterCreation.Id;
            //return Ok(model);
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            //return Ok(new { Message = "Student successfully added", Student = model });

        }

        [HttpPut]
        [Route("Update", Name = "UpdateStudentV2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateStudentAsync([FromBody] StudentDto? model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();


            var existingStudent = await _studentRepository.GetStudentByIdAsync(student => student.Id == model.Id, true);
            if (existingStudent == null)
                return NotFound($"Student with Id {model.Id} not found");
            var newRecord = _mapper.Map<Student>(model);

            await _studentRepository.UpdateStudentAsync(newRecord);

            return Ok("Student record updated successfully");
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial", Name = "UpdateStudentPartialV2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDto>? patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            var theStudentToUpdate = await _studentRepository.GetStudentByIdAsync(student => student.Id == id, true);
            if (theStudentToUpdate == null)
                return NotFound($"Student with Id {id} not found");

            var studentDto = _mapper.Map<StudentDto>(theStudentToUpdate);

            patchDocument.ApplyTo(studentDto, ModelState);
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            theStudentToUpdate = _mapper.Map<Student>(studentDto);

            await _studentRepository.UpdateStudentAsync(theStudentToUpdate);
            return Ok("Student record updated successfully");
        }
    }
}
