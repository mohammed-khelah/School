using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TestAppSchool.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace TestAppSchool.Data.Models
{
    public class Student
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? FatherName { get; set; }

        public string? GrandfatherName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string IdentityNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        [Required]
        public string? IdentityPhotoUrl { get; set; }

        public string? Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime DateRegisterSchool { get; set; }

        public Level? Level { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal? LastYearEstimate { get; set; }

        public DateTime? TimeRegister { get; set; }

        public bool IsDelete { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(x => x.IdentityNumber).IsUnique();
        }

        public Student()
        {
            Id = Guid.NewGuid().ToString();
            TimeRegister = DateTime.Now;
        }
        //[Index(IsUnique = true)]
        //[Remote("IsExist", "Place", ErrorMessage = "URL exist!")]
    }
}
