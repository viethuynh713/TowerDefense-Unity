using System;
using DG.Tweening;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using VContainer;

namespace MythicEmpire.UI.Menu
{
    public class LoginUI : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        #endregion

        #region Amination Field

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector2 _anchorPos;

        #endregion

        [Inject] private ILoginService _userLoginService;
        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _anchorPos = _rectTransform.anchoredPosition;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            EventManager.Instance.RegisterListener(EventID.OnLoginSuccess, LoadScene);
            EventManager.Instance.RegisterListener(EventID.OnRegisterSuccess, o => gameObject.SetActive(true));
        }

        private void LoadScene(object obj)
        {
            SceneManager.LoadSceneAsync("Lobby");
        }

        private void OnEnable()
        {
            OnAppear();
            _emailInputField.text = "";
            _passwordInputField.text = "";
        }
        
        public void OnAppear()
        {
            DOTween.KillAll(true);
            _rectTransform.anchoredPosition = _anchorPos;
            _canvasGroup.alpha = 0;
            transform.localScale = Vector3.one * 0.7f;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart);

            _canvasGroup.DOFade(1,0.3f).SetEase(Ease.InQuart);
        }

        public void OnDisappear()
        {
            _canvasGroup.alpha = 1;

            _rectTransform.DOLocalMoveX(_anchorPos.x -100, 0.5f).SetEase(Ease.OutQuart);
            _canvasGroup.DOFade(0, 0.7f).SetEase(Ease.OutQuart).OnComplete(
                () => gameObject.SetActive(false));

        }
        public void  LoginButtonClick()
        {
            if (string.IsNullOrEmpty(_emailInputField.text) || string.IsNullOrEmpty(_passwordInputField.text))
            {
                Notification.Instance.PopupNotifyWaring("Please fill in your email or password");
                return;
            }
            else if (_passwordInputField.text.Length < 6)
            {
                Notification.Instance.PopupNotifyWaring("Passwords must be at least 6 characters");
                return;
            }
            else
            {
                _userLoginService.Login(_emailInputField.text,_passwordInputField.text);
            }
        }
        

    }
}