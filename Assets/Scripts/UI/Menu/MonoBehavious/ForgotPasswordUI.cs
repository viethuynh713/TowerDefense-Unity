using TMPro;
using UnityEngine;

namespace MythicEmpire.UI.Menu
{
    public class ForgotPasswordUI:MonoBehaviour
    {
        private IForgotPasswordService _forgotPasswordService;
        [SerializeField] TMP_InputField _emailInputField;
        [SerializeField] TMP_InputField _optInputField;
        [SerializeField] TMP_InputField _newPasswordInputField;
        [SerializeField] TMP_InputField _confirmPasswordInputField;

        public void ForgotPasswordButtonClick()
        {
            
        }
        public void SendOTP()
        {
            
        }

        public void ConfirmReset()
        {
            
        }
    }
}