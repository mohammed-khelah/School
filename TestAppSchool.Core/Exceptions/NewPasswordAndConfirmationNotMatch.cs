namespace TestAppSchool.Core.Exceptions
{
    public class NewPasswordAndConfirmationNotMatch : Exception
    {
        public NewPasswordAndConfirmationNotMatch() : base("كلمة المرور الجديدة وتأكيد كلمة المرور الجديدة غير متطابقتين.")
        {
        }
    }
}
