using System;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace MythicEmpire.UI.Menu
{
    public class LoginUI : MonoBehaviour
    {
        private ILoginService _loginService;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        #region Amination Field

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector2 _anchorPos;

        #endregion
        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _anchorPos = _rectTransform.anchoredPosition;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            OnAppear();
        }

        // public void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         OnDisappear();
        //     }
        // }
        
        public void OnAppear()
        {
            DOTween.KillAll(true);
            _rectTransform.anchoredPosition = new Vector2(_anchorPos.x + 100, _anchorPos.y);
            _canvasGroup.alpha = 0;

            _rectTransform.DOLocalMoveX(_anchorPos.x, 0.5f).SetEase(Ease.InQuart);
            _canvasGroup.DOFade(1,0.5f).SetEase(Ease.InQuart);
        }

        public void OnDisappear()
        {
            DOTween.KillAll(true);

            _rectTransform.DOLocalMoveX(_anchorPos.x -100, 0.5f).SetEase(Ease.OutQuart);
            _canvasGroup.DOFade(0,0.5f).SetEase(Ease.OutQuart).OnComplete(
                ()=>this.gameObject.SetActive(false));
            // this.gameObject.SetActive(false);
        }
        public void  LoginButtonClick()
        {
            
        }
    }
}