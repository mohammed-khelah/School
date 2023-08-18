using TestAppSchool.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using TestAppSchool.Data;

namespace TestAppSchool.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly TestAppSchoolDbContext _db;
        protected readonly IUserService _userService;
        protected string userType;
        protected string userId;

        public BaseController(TestAppSchoolDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (User.Identity.IsAuthenticated)
            {
                var userActive = _db.Users.SingleOrDefault(x => x.UserName == User.Identity.Name && !x.IsDelete);
                if (userActive != null)
                {
                    userActive.IsActive = true;
                    _db.Users.Update(userActive);
                    _db.SaveChanges();
                }

                var userName = User.Identity.Name;
                var user = _userService.GetUserByUsername(userName);
                var studentVM = _userService.GetStudentVM(user.StudentId);

                userId = user.Id;
                userType = user.UserType;
                ViewBag.fullName = user.FullName;
                ViewBag.image = studentVM.ProfilePhotoUrl;
                ViewBag.UserType = user.UserType.ToString();
            }
        }
    }
}
