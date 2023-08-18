using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Core.Dtos
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Display(Name = "اسم الأب")]
        public string FatherName { get; set; }

        [Display(Name = "اسم الجد")]
        public string GrandfatherName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "اسم العائلة")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "رقم الهوية")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الجنس")]
        public string Gender { get; set; }

        [Display(Name = "صورة شخصية")]
        public IFormFile ProfilePhoto { get; set; }

        [Display(Name = "صورة الهوية")]
        public IFormFile IdentityPhoto { get; set; }

        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Phone]
        [Display(Name = "رقم الجوال")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime DOB { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الالتحاق بالمدرسة")]
        public DateTime DateRegisterSchool { get; set; }

        [Display(Name = "المستوى")]
        public Level Level { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "تقدير آخر سنة")]
        public decimal? LastYearEstimate { get; set; }
    }
}
