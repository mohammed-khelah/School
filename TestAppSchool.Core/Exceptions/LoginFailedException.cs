namespace TestAppSchool.Core.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("فشلت عملية تسجيل الدخول")
        {
        }
    }
}
