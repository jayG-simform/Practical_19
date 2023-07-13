using DataAccess.Interface;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practical_19.Repository;

namespace Practical_19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentRepo _context;

        public StudentController(IStudentRepo context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> GetStudents()
        {
            try
            {
                return Ok(await _context.GetAllStudents());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }
        }
        [HttpGet("{id:int}")]
        public ActionResult GetStudent(int id)
        {
            try
            {
                return Ok(_context.GetStudent(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult PutStudent(int id, Students student)
        {
            try
            {
                if (id != student.Id)
                {
                    return BadRequest("Id Mismatched");
                }
                var StUpdate = _context.GetStudent(id);
                if (StUpdate == null)
                {
                    return NotFound($"Student id={id} not found");
                }
                return Ok(_context.Edit(student));


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }
        }

        [HttpPost]
        public ActionResult PostStudent(Students student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (student == null) return BadRequest(ModelState);
                    int studentId = _context.Add(student);
                    student.Id = studentId;
                    return CreatedAtAction("GetStudents", new { id = studentId }, student);
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var empId = _context.GetStudent(id);
                if (empId == null)
                {
                    return NotFound($"Student id={id} not found");
                }
                return Ok(_context.Delete(id));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }
        }
    }
}
