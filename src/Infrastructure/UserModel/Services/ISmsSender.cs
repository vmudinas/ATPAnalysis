using System.Threading.Tasks;

namespace Infrastructure.UserModel.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}