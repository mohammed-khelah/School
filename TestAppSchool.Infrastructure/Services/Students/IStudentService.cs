using Microsoft.AspNetCore.Mvc.Rendering;
using TestAppSchool.Core.Dtos;

namespace TestAppSchool.Infrastructure.Services.Students
{
    public interface IStudentService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);

        Task<UpdateStudentDto> Get(string id);

        List<SelectListItem> GetLevelList();

        Task<string> Create(CreateStudentDto dto);

        Task<string> Update(UpdateStudentDto dto);

        Task<string> Delete(string id);
    }
}
