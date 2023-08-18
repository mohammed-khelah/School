using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestAppSchool.Core.Dtos;
using TestAppSchool.Infrastructure.Services.Users;
using TestAppSchool.Core.Constants;
using TestAppSchool.Infrastructure.Services.Students;
using TestAppSchool.Data;
using Microsoft.EntityFrameworkCore;
using TestAppSchool.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace TestAppSchool.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IStudentService _studentService;

        public UserController(TestAppSchoolDbContext db, IStudentService studentService, IUserService userService) : base(db, userService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetUserData(Pagination pagination, Query query)
        {
            var result = await _userService.GetAll(pagination, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["students"] = new SelectList(await _userService.GetStudentList(), "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (dto.RegisterType == 1)
            {
                var student = _db.Students.SingleOrDefault(x => !x.IsDelete && x.Id == dto.StudentId);
                if (student == null)
                {
                    throw new EntityNotFoundException();
                }

                dto.FirstName = student.FirstName;
                dto.FamilyName = student.FamilyName;
                dto.IdentityNumber = student.IdentityNumber;
                dto.PhoneNumber = student.PhoneNumber;

                ModelState.Remove("FirstName");
                ModelState.Remove("FamilyName");
                ModelState.Remove("PhoneNumber");
                ModelState.Remove("IdentityNumber");
            }
            else if (dto.RegisterType == 0)
            {
                ModelState.Remove("StudentId");
            }

            if (!ModelState.IsValid)
            {
                throw new OperationFailedException();
            }

            if (ModelState.IsValid)
            {
                await _userService.Create(dto);

                return Ok(Result.AddSuccessResult());
            }

            ViewData["students"] = new SelectList(await _userService.GetStudentList(), "Id", "FullName");

            return View(dto);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.Add(dto);

                return Ok(Result.AddSuccessResult());
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await _userService.Get(Id);
            var userDb = await _db.Users.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == Id);
            if (userDb == null)
            {
                throw new EntityNotFoundException();
            }

            var student = await _studentService.Get(user.StudentId);

            user.IdentityNumber = student.IdentityNumber;
            user.FirstName = userDb.FullName.Substring(0, userDb.FullName.IndexOf(" "));
            user.FamilyName = userDb.FullName.Substring(userDb.FullName.IndexOf(" "));

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto, string Id)
        {
            var user = await _userService.Get(Id);

            var student = await _studentService.Get(user.StudentId);

            dto.StudentId = student.Id;
            if (dto.IsResetPassword != 1)
            {
                ModelState.Remove("CurrentPassword");
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");
            }

            if (ModelState.IsValid)
            {
                await _userService.Update(dto);
                return Ok(Result.EditSuccessResult());
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            await _userService.Delete(Id);
            return Ok(Result.DeleteSuccessResult());
        }
    }
}

