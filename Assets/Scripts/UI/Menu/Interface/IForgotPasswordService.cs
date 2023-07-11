using System.Threading.Tasks;

namespace MythicEmpire
{
    public interface IForgotPasswordService
    {
        Task IsValidOtp(string email,string otp);
        Task ResetPassword(string email,string newPassword);
        Task SenOtp(string email);
    }
}