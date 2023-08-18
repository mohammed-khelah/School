namespace TestAppSchool.Core.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException() : base("فشلت العملية")
        {
        }
    }
}
