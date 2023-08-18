namespace TestAppSchool.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Task Send(string email, string subject, string htmlMessage);
    }
}
