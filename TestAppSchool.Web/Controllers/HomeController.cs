using TestAppSchool.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using TestAppSchool.Data;

namespace TestAppSchool.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(TestAppSchoolDbContext db, IUserService userService) : base(db, userService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}