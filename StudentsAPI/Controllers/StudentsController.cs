using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsAPI.Data;
using StudentsAPI.Models;

namespace StudentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<List<Students>>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<ActionResult<Students>> GetStudentById(int id)
        {
            var student = await _context.Students.Where(Student => Student.Id == id).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost("AddStudent")]
        public async Task<ActionResult> AddStudent(Students student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete("DeleteStudent/{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.Where(Student => Student.Id == id).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateStudent")]

        public async Task<ActionResult<Students>> UpdateStudent(Students updateStudent)
        {
            var dbStudent = await _context.Students.FindAsync(updateStudent.Id);
            if (dbStudent == null)
            {
                return NotFound("Student not found...");
            }
            dbStudent.Name = updateStudent.Name;
            dbStudent.Email = updateStudent.Email;

            await _context.SaveChangesAsync();

            return Ok(await _context.Students.ToListAsync());

        }

    }
}
