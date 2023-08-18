using System.ComponentModel.DataAnnotations;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Core.Dtos
{
    public class UpdateUserDto
    {
        public string Id { get; set; }

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
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الحالية")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(100, ErrorMessage = "يجب أن يكون طول {0} على الأقل {2} حروف والحد الأقصى {1} من الأحرف.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيد كلمة المرور الجديدة غير متطابقتين.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة")]
        public string? ConfirmNewPassword { get; set; }

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

        public string? StudentId { get; set; }

        [Display(Name = "هل تريد تغيير كلمة المرور؟")]
        public int IsResetPassword { get; set; }
    }
}
