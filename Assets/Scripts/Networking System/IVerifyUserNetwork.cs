using System.Collections;

namespace MythicEmpire.Networking
{
    public interface IVerifyUserNetwork
    {
        IEnumerator RegisterRequest(string nickName, string email, string password);
        IEnumerator LoginRequest(string email, string password);
        IEnumerator SendOTPRequest(string email);
        IEnumerator ResetPasswordRequest(string email,string newPassword);
    }
}