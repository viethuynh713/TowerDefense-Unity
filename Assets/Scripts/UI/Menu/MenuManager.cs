
using UnityEngine;
using DG.Tweening;
using MythicEmpire.CommonScript;

namespace MythicEmpire.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _loginButton;
        [SerializeField] private RectTransform _registerButton;
        [SerializeField] private RectTransform _settingButton;
        [SerializeField] private RectTransform _exitButton;
        
        private CanvasGroup _loginCanvasGrp;
        private CanvasGroup _registerCanvasGrp;
        private CanvasGroup _settingCanvasGrp;
        private CanvasGroup _exitCanvasGrp;

        private Vector2 _loginPos;
        private Vector2 _registerPos;
        private Vector2 _settingPos;
        private Vector2 _exitPos;

        private void Awake()
        {
            _loginCanvasGrp = _loginButton.GetComponent<CanvasGroup>();
            _registerCanvasGrp = _registerButton.GetComponent<CanvasGroup>();
            _settingCanvasGrp = _settingButton.GetComponent<CanvasGroup>();
            _exitCanvasGrp = _exitButton.GetComponent<CanvasGroup>();

            _loginPos = _loginButton.anchoredPosition;
            _registerPos = _registerButton.anchoredPosition;
            _settingPos = _settingButton.anchoredPosition;
            _exitPos = _exitButton.anchoredPosition;
        }

        private void OnEnable()
        {
            // DOTween.KillAll(true);

            _loginCanvasGrp.alpha = 0;
            _loginButton.anchoredPosition = new Vector2(_loginPos.x - 100, _loginPos.y);
            _loginButton.DOLocalMoveX(_loginPos.x, 0.5f).SetEase(Ease.InOutBack);
            _loginCanvasGrp.DOFade(1, 0.4f);

            _registerCanvasGrp.alpha = 0;
            _registerButton.anchoredPosition = new Vector2(_registerPos.x + 100, _registerPos.y);
            _registerButton.DOLocalMoveX(_registerPos.x, 0.5f).SetEase(Ease.InOutBack);
            _registerCanvasGrp.DOFade(1, 0.4f);

            _settingCanvasGrp.alpha = 0;
            _settingButton.anchoredPosition = new Vector2(_settingPos.x - 100, _settingPos.y);
            _settingButton.DOLocalMoveX(_settingPos.x, 0.5f).SetEase(Ease.InOutBack);
            _settingCanvasGrp.DOFade(1, 0.4f);

            _exitCanvasGrp.alpha = 0;
            _exitButton.anchoredPosition = new Vector2(_exitPos.x + 100, _exitPos.y);
            _exitButton.DOLocalMoveX(_exitPos.x, 0.5f).SetEase(Ease.InOutBack);
            _exitCanvasGrp.DOFade(1, 0.4f);



        }

        public void LoginButtonClick()
        {

        }

        public void RegisterButtonClick()
        {

        }

        public void SettingButtonClick()
        {

        }

        public void ExitButtonClick()
        {
            Common.Log("Quiz game");
            Application.Quit();
        }
    }
}
