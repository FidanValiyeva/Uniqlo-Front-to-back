using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using UniqloMVC5.Helpers;
using UniqloMVC5.Services.Abstracts;

namespace UniqloMVC5.Services.Implements
{
    public class EmailService : IEmailService
    {
        readonly SmtpClient _client;
        readonly MailAddress _from;
        readonly HttpContext Context;
        public EmailService(IOptions<SmtpOptions>option,IHttpContextAccessor acc)
        {
            var opt = option.Value;
            _client = new();
            _client.Host = opt.Host;
            _client.Port = opt.Port;
            _client.Credentials = new NetworkCredential(opt.Username, opt.Password);
            _client.EnableSsl=true;
            _from = new MailAddress(opt.Username, "Uniqlo");
            Context = acc.HttpContext;
        }

        public void SendEmailConfirmation(string? reciever, string name, string token)
        {
           MailAddress to = new(reciever);
           MailMessage msg = new MailMessage(_from,to);

            msg.IsBodyHtml = true;
            msg.Subject = "Confirm your email address";
            string Url = Context.Request.Scheme + "://" + Context.Request.Host + "/Account/" +
             "VerifyEmail?token=" + token+"&user="+name;           
            msg.Body = EmailTemplates.VerifyEmail.Replace("__$name", name).Replace("__$link", Url);          
             _client.Send(msg);

            
        }
    }
}
