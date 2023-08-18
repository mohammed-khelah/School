using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestAppSchool.Core.Dtos;
using TestAppSchool.Core.Exceptions;
using TestAppSchool.Core.ViewModels;
using TestAppSchool.Data;
using TestAppSchool.Data.Models;
using Microsoft.AspNetCore.Http;
using TestAppSchool.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestAppSchool.Infrastructure.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly TestAppSchoolDbContext _db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public StudentService(TestAppSchoolDbContext db, IMapper mapper, IWebHostEnvironment env)
        {
            _db = db;
            _mapper = mapper;
            _env = env;
        }

        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Students.OrderByDescending(x => x.TimeRegister).Where(x => !x.IsDelete).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var posts = _mapper.Map<List<StudentViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = posts,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };

            return result;
        }

        public async Task<UpdateStudentDto> Get(string id)
        {
            var student = await _db.Students.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<UpdateStudentDto>(student);
        }

        public List<SelectListItem> GetLevelList()
        {
            var levelList = Enum.GetValues(typeof(Level)).OfType<Level>().ToList();
            var selectList = levelList.Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() }).ToList();
            return selectList;
        }

        public async Task<string> Create(CreateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);

            student.ProfilePhotoUrl = await SaveImage(dto.ProfilePhoto, "Images");
            student.IdentityPhotoUrl = await SaveImage(dto.IdentityPhoto, "Images");

            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();

            return student.Id;
        }
        
        public async Task<string> Update(UpdateStudentDto dto)
        {
            var student = await _db.Students.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            var updatedStudent = _mapper.Map(dto, student);

            if (dto.ProfilePhoto != null)
            {
                student.ProfilePhotoUrl = await SaveImage(dto.ProfilePhoto, "Images");
            }
            
            if (dto.IdentityPhoto != null)
            {
                student.IdentityPhotoUrl = await SaveImage(dto.IdentityPhoto, "Images");
            }
            
            _db.Students.Update(student);
            await _db.SaveChangesAsync();

            return student.Id;
        }

        public async Task<string> Delete(string id)
        {
            var student = await _db.Students.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            student.IsDelete = true;
            _db.Students.Update(student);
            await _db.SaveChangesAsync();

            return student.Id;
        }

        private async Task<string> SaveImage(IFormFile file,  string imagePath)
        {
            string fileName = null;
            if (file != null && file.Length > 0)
            {

                var uploads = Path.Combine(_env.WebRootPath, imagePath);
                fileName = file.FileName + "_" + Guid.NewGuid().ToString().Substring(0, 9).Replace("-", "")
                    + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-tt") + Path.GetExtension(file.FileName);
                await using var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
