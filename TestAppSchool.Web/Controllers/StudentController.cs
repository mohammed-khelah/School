using TestAppSchool.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using TestAppSchool.Core.Dtos;
using TestAppSchool.Core.Constants;
using TestAppSchool.Infrastructure.Services.Students;
using TestAppSchool.Data;

namespace TestAppSchool.Web.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(TestAppSchoolDbContext db, IStudentService studentService, IUserService userService) : base(db, userService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetStudentData(Pagination pagination, Query query)
        {
            var result = await _studentService.GetAll(pagination, query);
            return Json(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.LevelList = _studentService.GetLevelList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateStudentDto dto)
        {
            if (ModelState.IsValid)
            {
                await _studentService.Create(dto);
                return Ok(Result.AddSuccessResult());
            }

            ViewBag.LevelList = _studentService.GetLevelList();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var student = await _studentService.Get(Id);

            ViewBag.LevelList = _studentService.GetLevelList();
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateStudentDto dto)
        {
            if (ModelState.IsValid)
            {
                await _studentService.Update(dto);
                return Ok(Result.EditSuccessResult());
            }

            ViewBag.LevelList = _studentService.GetLevelList();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            await _studentService.Delete(Id);
            return Ok(Result.DeleteSuccessResult());
        }
    }
}
