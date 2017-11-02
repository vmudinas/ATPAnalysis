using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.UserModel.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(List<string> emailList, string subject, string emailMessage, string adminUserName,
            string adminEmail,
            string adminPassword, string smtpServer, int smtpServerPort);

        Task SendEmailAsync(string email, string subject, string emailMessage, string adminUserName, string adminEmail,
            string adminPassword, string smtpServer, int smtpServerPort);
        Task SendHtmlEmailAsync(List<string> emailList, string subject, string pathToFile, string adminUserName,
           string adminEmail,
           string adminPassword, string smtpServer, int smtpServerPort);

        Task SendHtmlEmailAsync(string email, string subject, string pathToFile, string adminUserName, string adminEmail,
            string adminPassword, string smtpServer, int smtpServerPort);

        
    }
}