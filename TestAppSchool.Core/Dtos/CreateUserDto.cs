using System.ComponentModel.DataAnnotations;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Core.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "اسم العائلة")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(100, ErrorMessage = "يجب أن يكون طول {0} على الأقل {2} حروف والحد الأقصى {1} من الأحرف.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وكلمة المرور التأكيدية غير متطابقتين.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Phone]
        [Display(Name = "رقم الجوال")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "رقم الهوية")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "نوع المستخدم")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string StudentId { get; set; }

        public int RegisterType { get; set; }
    }
}
