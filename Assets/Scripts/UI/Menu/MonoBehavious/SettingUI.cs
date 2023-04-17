using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace MythicEmpire.UI.Menu
{    
    public class SettingData
    {
        public float EffectSound;
        public float MusicSound;
        public bool IsFullScreen;
    }
    [RequireComponent(typeof(CanvasGroup))]

    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider _effectSoundSlider;
        [SerializeField] private Slider _musicSoundSlider;
        [SerializeField] private Toggle _fullscreemOption;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _anchorPosition;
        private SettingData _settingData;
        
        private void OnEnable()
        {
            OnAppear();
        }
        private void Awake()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _anchorPosition = _rectTransform.anchoredPosition;
            
            if (PlayerPrefs.HasKey("Setting-Config"))
            {
                _settingData = JsonConvert.DeserializeObject<SettingData>(PlayerPrefs.GetString("Setting-Config"));
                _fullscreemOption.isOn = _settingData.IsFullScreen;
                _musicSoundSlider.value = _settingData.MusicSound;
                _effectSoundSlider.value = _settingData.EffectSound;

            }
            else
            {
                _settingData = new SettingData();
                _settingData.EffectSound = 1;
                _settingData.MusicSound = 1;
                _settingData.IsFullScreen = true;
                var data = JsonConvert.SerializeObject(_settingData);
                PlayerPrefs.SetString("Setting-Config",data);
                _fullscreemOption.isOn = _settingData.IsFullScreen;
                _musicSoundSlider.value = _settingData.MusicSound;
                _effectSoundSlider.value = _settingData.EffectSound;
            }
            _fullscreemOption.onValueChanged.AddListener(OnFullScreenChange);
            _effectSoundSlider.onValueChanged.AddListener(OnEffectSoundChange);
            _musicSoundSlider.onValueChanged.AddListener(OnMusicSoundChange);
        }

        private void OnMusicSoundChange(float value)
        {
            _settingData.MusicSound = value;
            var data = JsonConvert.SerializeObject(_settingData);
            PlayerPrefs.SetString("Setting-Config",data);
        }

        private void OnEffectSoundChange(float value)
        {
            _settingData.EffectSound = value;
            var data = JsonConvert.SerializeObject(_settingData);
            PlayerPrefs.SetString("Setting-Config",data);
            
        }

        private void OnFullScreenChange(bool value)
        {
            _settingData.IsFullScreen = value;
            Screen.fullScreen = value;
            var data = JsonConvert.SerializeObject(_settingData);
            PlayerPrefs.SetString("Setting-Config",data);
            
        }

        public void OnAppear()
        {
            DOTween.KillAll(true);
            _rectTransform.anchoredPosition = _anchorPosition;
            _canvasGroup.alpha = 0;
            transform.localScale = Vector3.one * 0.7f;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuart);

            _canvasGroup.DOFade(1,0.3f).SetEase(Ease.InQuart);
        }

        public void OnDisappear()
        {
            _canvasGroup.alpha = 1;

            _rectTransform.DOLocalMoveX(_anchorPosition.x -100, 0.5f).SetEase(Ease.OutQuart);
            _canvasGroup.DOFade(0, 0.7f).SetEase(Ease.OutQuart).OnComplete(
                () => gameObject.SetActive(false));

        }
        
        
    }
}