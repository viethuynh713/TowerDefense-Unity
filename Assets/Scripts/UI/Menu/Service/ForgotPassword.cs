using System.Threading.Tasks;
using MythicEmpire;
using MythicEmpire.Networking;
using VContainer;

namespace MythicEmpire.UI.Menu
{
    public class ForgotPassword : IForgotPasswordService
    {
        [Inject] private IVerifyUserNetwork _verifyUserNetwork;


        public async Task IsValidOtp(string email, string otp)
        {
            await _verifyUserNetwork.ConfirmOTPRequest(email, otp);
        }

        public Task ResetPassword(string email, string newPassword)
        {
            _verifyUserNetwork.ResetPasswordRequest(email, newPassword);
            return Task.CompletedTask;
        }

        public async Task SenOtp(string email)
        {
            await _verifyUserNetwork.SendOTPRequest(email);
        }
    }
}