using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAppSchool.Data.Models;

namespace TestAppSchool.Data
{
    public class TestAppSchoolDbContext : IdentityDbContext<User>
    {
        public TestAppSchoolDbContext(DbContextOptions<TestAppSchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        // add-migration init_db    in Default project TestAppSchool.Data; because models in TestAppSchool.Data
        // update-database      in Default project TestAppSchool.Web; because Connection to database in TestAppSchool.Web
    }
}