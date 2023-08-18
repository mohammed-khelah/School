using TestAppSchool.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Data
{
    public static class DbSeeder
    {

        public static IHost SeedDb(this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<TestAppSchoolDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                //context.SeedStudent().Wait();
                userManager.SeedUser().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return webHost;
        }

        //public static async Task SeedStudent(this TestAppSchoolDbContext _db)
        //{
        //    if (await _db.Students.AnyAsync())
        //    {
        //        return;
        //    }

        //    var students = new List<Student>();

        //    var student = new Student();
        //    student.FirstName = "Student";
        //    student.FamilyName = "Student";
        //    student.IdentityNumber = "123456789";
        //    student.Gender = "Male";
        //    student.PhoneNumber = "987654321";

        //    //var student1 = new Student();
        //    //student1.FirstName = "User";
        //    //student1.FamilyName = "123";
        //    //student1.IdentityNumber = "789456123";
        //    //student1.Gender = "Male";
        //    //student1.PhoneNumber = "963852741";

        //    students.Add(student);
        //    //students.Add(student1);

        //    await _db.Students.AddRangeAsync(students);
        //    await _db.SaveChangesAsync();
        //}

        static readonly TestAppSchoolDbContext _dbs;
        public static async Task SeedUser(this UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var student = new Student();
            student.FirstName = "Student";
            student.FamilyName = "Admin";
            student.IdentityNumber = "987654321";
            student.Gender = "Male";
            student.PhoneNumber = "123456789";

            var user = new User();
            user.FullName = student.FirstName + " " + student.FamilyName;
            user.UserType = UserType.Admin;
            user.Email = "admin123@gmail.com";
            user.UserName = user.Email;
            user.StudentId = student.Id;

            _dbs.Students.AddRange(student);
            _dbs.SaveChanges();

            await userManager.CreateAsync(user, "Admin111$$");
        }
    }
}
