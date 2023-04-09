using UnityEngine;
using TMPro;
namespace MythicEmpire.UI.Menu
{
    public class RegisterUI : MonoBehaviour
    {
        private IRegisterService _registerService;
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _confirmPwInputField;

        public void RegisterButtonClick()
        {
            
        }
    }
}