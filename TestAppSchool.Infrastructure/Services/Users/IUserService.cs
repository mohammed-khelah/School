using TestAppSchool.Core.Dtos;
using TestAppSchool.Core.ViewModels;

namespace TestAppSchool.Infrastructure.Services.Users
{
    public interface IUserService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);

        UserViewModel GetUserByUsername(string username);

        Task<UpdateUserDto> Get(string Id);

        StudentViewModel GetStudentVM(string Id);

        Task<List<StudentViewModel>> GetStudentList();

        Task<string> Create(CreateUserDto dto);

        Task<string> Add(AddStudentDto dto);

        Task<string> Update(UpdateUserDto dto);

        Task<string> Delete(string Id);
    }
}
