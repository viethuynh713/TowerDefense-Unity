namespace MythicEmpire
{
    public interface IForgotPasswordService
    {
        void IsValidOtp(string otp);
        void ResetPassword(string newPassword);
        void SenOtp(string email);
    }
}