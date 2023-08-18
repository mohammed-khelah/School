using AutoMapper;
using TestAppSchool.Core.Dtos;
using TestAppSchool.Core.ViewModels;
using TestAppSchool.Data.Models;

namespace TestAppSchool.Infrastructure.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>().ForMember(x => x.UserType, x => x.MapFrom(x => x.UserType.ToString()))
                .ForMember(x => x.TimeRegister, x => x.MapFrom(x => x.TimeRegister.ToString("yyyy/MM/dd")))
                .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive.ToString()));
            CreateMap<UserViewModel, User>();
            CreateMap<Student, UserViewModel>();
            CreateMap<UserViewModel, Student>();
            CreateMap<CreateUserDto, User>();
            CreateMap<AddStudentDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserDto>();

            CreateMap<CreateUserDto, CreateStudentDto>().ForMember(x => x.ProfilePhoto, x => x.Ignore())
                .ForMember(x => x.IdentityPhoto, x => x.Ignore());
            CreateMap<CreateUserDto, StudentViewModel>();

            CreateMap<Student, StudentViewModel>().ForMember(x => x.Level, x => x.MapFrom(x => x.Level.ToString()))
                .ForMember(x => x.DOB, x => x.MapFrom(x => x.DOB.ToString("yyyy/MM/dd")))
                .ForMember(x => x.DateRegisterSchool, x => x.MapFrom(x => x.DateRegisterSchool.ToString("yyyy/MM/dd")))
                .ForMember(x => x.FullName, x => x.MapFrom(x => x.FirstName + " " + x.FamilyName));
            CreateMap<CreateStudentDto, Student>().ForMember(x => x.ProfilePhotoUrl, x => x.Ignore())
                .ForMember(x => x.IdentityPhotoUrl, x => x.Ignore());
            CreateMap<UpdateStudentDto, Student>().ForMember(x => x.ProfilePhotoUrl, x => x.Ignore())
                .ForMember(x => x.IdentityPhotoUrl, x => x.Ignore());
            CreateMap<Student, UpdateStudentDto>().ForMember(x => x.ProfilePhoto, x => x.Ignore())
                .ForMember(x => x.IdentityPhoto, x => x.Ignore()); ;
        }
    }
}
