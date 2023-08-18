namespace TestAppSchool.Core.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public string UserType { get; set; }

        public string TimeRegister { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string StudentId { get; set; }

        public StudentViewModel Student { get; set; }
    }
}
