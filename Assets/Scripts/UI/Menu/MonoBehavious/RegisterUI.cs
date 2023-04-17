using System;
using DG.Tweening;
using MythicEmpire.Manager;
using MythicEmpire.PlayerInfos;
using UnityEngine;
using TMPro;
using VContainer;

namespace MythicEmpire.UI.Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RegisterUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _confirmPwInputField;

        [Inject]private IRegisterService _registerService;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _anchorPosition;
        

        private void Awake()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _anchorPosition = _rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            _nicknameInputField.text = "";
            _emailInputField.text = "";
            _passwordInputField.text = "";
            _confirmPwInputField.text = "";
            OnAppear();
        }

        public void OnAppear()
        {
            DOTween.KillAll(true);
            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = _anchorPosition;
            transform.localScale =  Vector3.one * 0.7f;;
            _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InQuart);
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart);
        }

        public void OnDisappear()
        {
            _canvasGroup.DOFade(0, 0.5f);
            _rectTransform.DOAnchorPosX(_anchorPosition.x - 100, 0.5f).SetEase(Ease.OutQuart).OnComplete(
                () => this.gameObject.SetActive(false));
        }

        public void RegisterButtonClick()
        {
            if (string.IsNullOrEmpty(_nicknameInputField.text)
                ||string.IsNullOrEmpty(_emailInputField.text)
                ||string.IsNullOrEmpty(_passwordInputField.text)
                ||string.IsNullOrEmpty(_confirmPwInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Please fill in your information");
                return;
            }

            if (_passwordInputField.text.Length < 6)
            {
                Notification.Instance.PopupNotifyWaring("Passwords must be at least 6 characters");
                return;
            }
            if (!_passwordInputField.text.Equals(_confirmPwInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Password and confirm password does not match");
                return;
            }
            _registerService.Register(new UserModel());
        }
    }
}