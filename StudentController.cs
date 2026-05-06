using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCRUDapplication.Models;

namespace StudentCRUDapplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext _studentDbContext;

        public StudentController(StudentDBContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _studentDbContext.Students.ToListAsync();
            return Ok(students);
        }

        // 🔍 SEARCH
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search text is required");
            }

            var students = await _studentDbContext.Students
                .Where(s => s.Name.Contains(name))
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound("No students found");
            }

            return Ok(students);
        }

        // ➕ ADD
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent([FromBody] Student objStudent)
        {
            if (objStudent == null)
            {
                return BadRequest();
            }

            _studentDbContext.Students.Add(objStudent);
            await _studentDbContext.SaveChangesAsync();

            return Ok(objStudent);
        }

        // ✏️ UPDATE
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, [FromBody] Student objStudent)
        {
            if (id != objStudent.Id)
            {
                return BadRequest("Student ID mismatch");
            }

            var existingStudent = await _studentDbContext.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return NotFound($"Student with Id = {id} not found");
            }

            existingStudent.Name = objStudent.Name;
            existingStudent.Course = objStudent.Course;

            await _studentDbContext.SaveChangesAsync();

            return Ok("Student updated successfully");
        }

        // ❌ DELETE by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _studentDbContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound($"Student with Id = {id} not found");
            }

            _studentDbContext.Students.Remove(student);
            await _studentDbContext.SaveChangesAsync();

            return Ok("Student deleted successfully");
        }

        // ❌ DELETE by NAME
        [HttpDelete("byname/{name}")]
        public async Task<ActionResult> DeleteByName(string name)
        {
            var students = await _studentDbContext.Students
                .Where(s => s.Name == name)
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound("No students found with that name");
            }

            _studentDbContext.Students.RemoveRange(students);
            await _studentDbContext.SaveChangesAsync();

            return Ok("Student(s) deleted successfully");
        }
    }
}