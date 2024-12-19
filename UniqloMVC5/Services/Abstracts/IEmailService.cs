namespace UniqloMVC5.Services.Abstracts
{
    public interface IEmailService
    {
       void SendEmailConfirmation(string? email, string userName, string token);
        //void SendEmailConfirmationAsync(string reciever,string name,string token );
    }
}
