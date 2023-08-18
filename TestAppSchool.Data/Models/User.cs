using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestAppSchool.Core.Enums;

namespace TestAppSchool.Data.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public UserType UserType { get; set; }

        public DateTime TimeRegister { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        public bool IsDelete { get; set; }

        public User()
        {
            TimeRegister = DateTime.Now;
        }
    }
}
