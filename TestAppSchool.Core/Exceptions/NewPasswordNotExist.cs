namespace TestAppSchool.Core.Exceptions
{
    public class NewPasswordNotExist : Exception
    {
        public NewPasswordNotExist() : base("كلمة المرور الجديدة غير موجودة")
        {
        }
    }
}
