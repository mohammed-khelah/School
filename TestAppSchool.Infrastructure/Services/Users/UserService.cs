using AutoMapper;
using TestAppSchool.Core.Dtos;
using TestAppSchool.Core.Exceptions;
using TestAppSchool.Core.ViewModels;
using TestAppSchool.Data;
using TestAppSchool.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestAppSchool.Infrastructure.Services.Email;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Infrastructure.Services.Users
{
    public class UserService : IUserService
    {
        private readonly TestAppSchoolDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public UserService(TestAppSchoolDbContext db, IMapper mapper, UserManager<User> userManager, IEmailService emailService)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Users.Include(x => x.Student).OrderByDescending(x => x.TimeRegister).Where(x => !x.IsDelete && (x.FullName.Contains(query.GeneralSearch) || string.IsNullOrWhiteSpace(query.GeneralSearch) || x.Email.Contains(query.GeneralSearch) || x.PhoneNumber.Contains(query.GeneralSearch))).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var users = _mapper.Map<List<UserViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = users,
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

        public UserViewModel GetUserByUsername(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == username && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UpdateUserDto> Get(string Id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateUserDto>(user);
        }

        public StudentViewModel GetStudentVM(string Id)
        {
            var student = _db.Students.SingleOrDefault(x => x.Id == Id && !x.IsDelete);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<StudentViewModel>(student);
        }

        public async Task<List<StudentViewModel>> GetStudentList()
        {
            var listStudent = await _db.Students.Where(x => !x.IsDelete).ToListAsync();
            var studentsVM = _mapper.Map<List<StudentViewModel>>(listStudent);
            var listUsers = await _db.Users.Where(x => !x.IsDelete).ToListAsync();
            var usersVM = _mapper.Map<List<UserViewModel>>(listUsers);
            var studentsHasAccount = new List<StudentViewModel>(studentsVM);

            foreach (var student in studentsVM)
            {
                foreach (var users in usersVM)
                {
                    if (student.Id == users.StudentId)
                    {
                        studentsHasAccount.Remove(student);
                    }
                }
            }

            return studentsHasAccount;
        }

        public async Task<string> Create(CreateUserDto dto)
        {
            var emailOrPhoneIsExist = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));
            if (emailOrPhoneIsExist)
            {
                throw new DuplicateEmailOrPhoneException();
            }

            var user  = _mapper.Map<User>(dto);

            if (dto.StudentId == null)
            {
                var student = new Student();
                student.FirstName = dto.FirstName;
                student.FamilyName = dto.FamilyName;
                student.IdentityNumber = dto.IdentityNumber;
                student.Gender = "";
                student.IdentityPhotoUrl = "";
                student.PhoneNumber = dto.PhoneNumber;
                user.StudentId = student.Id;

                await _db.Students.AddAsync(student);
                await _db.SaveChangesAsync();
            }

            user.UserName = dto.Email;
            user.FullName = dto.FirstName + " " + dto.FamilyName;

            var password = dto.Password;

            try
            {
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new OperationFailedException();
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailedException();
            }

            return user.Id;
        }
        
        public async Task<string> Add(AddStudentDto dto)
        {
            var emailOrPhoneIsExist = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));
            if (emailOrPhoneIsExist)
            {
                throw new DuplicateEmailOrPhoneException();
            }

            var user  = _mapper.Map<User>(dto);

            var student = new Student();
            student.FirstName = dto.FirstName;
            student.FamilyName = dto.FamilyName;
            student.IdentityNumber = dto.IdentityNumber;
            student.PhoneNumber = dto.PhoneNumber;
            student.Gender ="";
            student.IdentityPhotoUrl = "";

            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();

            user.StudentId = student.Id;
            user.UserName = dto.Email;
            user.FullName = dto.FirstName + " " + dto.FamilyName;
            user.UserType = UserType.Student;

            var password = GenratePassword();
            await _emailService.Send(dto.Email, "Register New Student", $"<h3>بيانات تسجيل دخول طالب جديد</h3><br /><br />" +
                $"<p>Email: {dto.Email}</p><br />Password: {password}");

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new OperationFailedException();
            }

            return user.Id;
        }

        public async Task<string> Update(UpdateUserDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            if (user.Email != dto.Email)
            {
                var emailOrPhoneIsExist = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));
                if (emailOrPhoneIsExist)
                {
                    throw new DuplicateEmailOrPhoneException();
                }
            }

            var updatedUser = _mapper.Map(dto, user);

            var student = await _db.Students.SingleOrDefaultAsync(x => x.Id == dto.StudentId && !x.IsDelete);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }

            student.FirstName = dto.FirstName;
            student.FamilyName = dto.FamilyName;
            student.IdentityNumber = dto.IdentityNumber;
            student.PhoneNumber = dto.PhoneNumber;

            _db.Students.Update(student);
            await _db.SaveChangesAsync();

            updatedUser.StudentId = student.Id;
            updatedUser.UserName = dto.Email;
            updatedUser.FullName = dto.FirstName + " " + dto.FamilyName;

            IdentityResult result;
            if (dto.IsResetPassword == 1)
            {
                if (!string.IsNullOrWhiteSpace(dto.NewPassword) && dto.NewPassword == dto.ConfirmNewPassword)
                {
                    result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                    if (!result.Succeeded)
                    {
                        throw new OperationFailedException();
                    }
                }
            }
                
            result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new OperationFailedException();
            }

            return user.Id;
        }

        public async Task<string> Delete(string Id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            
            var student = await _db.Students.SingleOrDefaultAsync(x => x.Id == user.StudentId && !x.IsDelete);
            if (student != null)
            {
                student.IsDelete = true;

                _db.Students.Update(student);
                await _db.SaveChangesAsync();
            }

            user.IsDelete = true;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            
            return user.Id;
        }

        private string GenratePassword()
        {
            string guidPassword = Guid.NewGuid().ToString().Substring(1, 9);
            string password = guidPassword.Substring(0, 2) + "-" + guidPassword.Substring(2);
            return password;
        }
    }
}
