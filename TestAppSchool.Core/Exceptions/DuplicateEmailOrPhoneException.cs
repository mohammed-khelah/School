namespace TestAppSchool.Core.Exceptions
{
    public class DuplicateEmailOrPhoneException : Exception
    {
        public DuplicateEmailOrPhoneException() : base("البريد الإلكتروني أو رقم الهاتف مكرر")
        {
        }
    }
}
