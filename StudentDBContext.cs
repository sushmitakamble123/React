using Microsoft.EntityFrameworkCore;

namespace StudentCRUDapplication.Models
{
    public class StudentDBContext : DbContext
    {
        public StudentDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
               "Server=localhost\\SQLEXPRESS;Database=Student_data;Trusted_Connection=True;TrustServerCertificate=True;"
           );
        }
    }
}