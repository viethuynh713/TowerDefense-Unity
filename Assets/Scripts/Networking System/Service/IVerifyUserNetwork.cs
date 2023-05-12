using System.Collections;
using System.Threading.Tasks;

namespace MythicEmpire.Networking
{
    public interface IVerifyUserNetwork
    {
        Task RegisterRequest(string nickName, string email, string password);
        Task LoginRequest(string email, string password);
        Task SendOTPRequest(string email);
        
        Task ConfirmOTPRequest(string email, string otp);
        Task ResetPasswordRequest(string email,string newPassword);
    }
}