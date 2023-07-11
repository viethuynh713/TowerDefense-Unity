using System;
using System.Collections;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using TMPro;
using UnityEngine;
using VContainer;

namespace MythicEmpire.UI.Menu
{
    public class ForgotPasswordUI:MonoBehaviour
    {
        [Inject] private IForgotPasswordService _forgotPasswordService;
        [SerializeField] TMP_InputField _emailInputField;
        [SerializeField] TMP_InputField _optInputField;
        [SerializeField] TMP_InputField _newPasswordInputField;
        [SerializeField] TMP_InputField _confirmPasswordInputField;

        [Header("Panel")]
        [SerializeField] private TranslateUI _emailPanel;
        [SerializeField] private TranslateUI _OTPPanel;
        [SerializeField] private TranslateUI _ResetPasswordPanel;
        [SerializeField] private GameObject _InitPanel;

        private void Start()
        {
            EventManager.Instance.RegisterListener(EventID.ResetPasswordSuccess, (o)=>
            {
                _ResetPasswordPanel.OnDisappear();
                _InitPanel.SetActive(true);
                gameObject.SetActive(false);
                Notification.Instance.NotifyStatus("Reset password successfully");
            });
            
            EventManager.Instance.RegisterListener(EventID.SendOTPSuccess, (o) =>
            {
                _emailPanel.OnDisappear();
                _OTPPanel.gameObject.SetActive(true);
            });            
            
            EventManager.Instance.RegisterListener(EventID.ConfirmOTPSuccess, (o) =>
            {
                _OTPPanel.OnDisappear();
                _ResetPasswordPanel.gameObject.SetActive(true);
            });
        }

        private void OnEnable()
        {
            _emailPanel.gameObject.SetActive(true);
            _OTPPanel.gameObject.SetActive(false);
            _ResetPasswordPanel.gameObject.SetActive(false);
            _emailInputField.text = "";
            _optInputField.text = "";
            _newPasswordInputField.text = "";
            _confirmPasswordInputField.text = "";
        }

        public void SendOTPButtonClick()
        {
            if (string.IsNullOrEmpty(_emailInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Please fill your email");
                return;
            }

            _forgotPasswordService.SenOtp(_emailInputField.text);
        }
        

        public void VerifyOTPButtonClick()
        {
            if (string.IsNullOrEmpty(_optInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Please fill OTP");
                return;
            }

            _forgotPasswordService.IsValidOtp(_emailInputField.text,_optInputField.text);

        }
        
        public void ResetPasswordButtonClick()
        {
            if (string.IsNullOrEmpty(_newPasswordInputField.text) || string.IsNullOrEmpty(_confirmPasswordInputField.text) )
            {
                Notification.Instance.PopupNotifyWaring("Please fill new password");
                return;
            }
            if (_newPasswordInputField.text.Length < 6)
            {
                Notification.Instance.PopupNotifyWaring("Passwords must be at least 6 characters");
                return;
            }
            if (!_newPasswordInputField.text.Equals(_confirmPasswordInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Password and confirm password does not match");
                return;
            }

            _forgotPasswordService.ResetPassword(_emailInputField.text,_newPasswordInputField.text);
        }

        public void ResendOtp()
        {
            Notification.Instance.NotifyStatus("Resend OTP successfully");
            _forgotPasswordService.SenOtp(_emailInputField.text);

        }
    }
}