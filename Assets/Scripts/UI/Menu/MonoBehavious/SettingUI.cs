using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace MythicEmpire.UI.Menu
{    
    public class SettingDataModel
    {
        public float EffectSoundVolume;
        public float MusicSoundVolume;
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
        private SettingDataModel _settingDataModel;
        
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
                _settingDataModel = JsonConvert.DeserializeObject<SettingDataModel>(PlayerPrefs.GetString("Setting-Config"));
                _fullscreemOption.isOn = _settingDataModel.IsFullScreen;
                _musicSoundSlider.value = _settingDataModel.MusicSoundVolume;
                _effectSoundSlider.value = _settingDataModel.EffectSoundVolume;

            }
            else
            {
                _settingDataModel = new SettingDataModel();
                _settingDataModel.EffectSoundVolume = 1;
                _settingDataModel.MusicSoundVolume = 1;
                _settingDataModel.IsFullScreen = true;
                var data = JsonConvert.SerializeObject(_settingDataModel);
                PlayerPrefs.SetString("Setting-Config",data);
                _fullscreemOption.isOn = _settingDataModel.IsFullScreen;
                _musicSoundSlider.value = _settingDataModel.MusicSoundVolume;
                _effectSoundSlider.value = _settingDataModel.EffectSoundVolume;
            }
            _fullscreemOption.onValueChanged.AddListener(OnFullScreenOptionChange);
            _effectSoundSlider.onValueChanged.AddListener(OnEffectSoundChange);
            _musicSoundSlider.onValueChanged.AddListener(OnMusicSoundChange);
        }

        private void OnMusicSoundChange(float value)
        {
            _settingDataModel.MusicSoundVolume = value;
            var data = JsonConvert.SerializeObject(_settingDataModel);
            PlayerPrefs.SetString("Setting-Config",data);
        }

        private void OnEffectSoundChange(float value)
        {
            _settingDataModel.EffectSoundVolume = value;
            var data = JsonConvert.SerializeObject(_settingDataModel);
            PlayerPrefs.SetString("Setting-Config",data);
            
        }

        private void OnFullScreenOptionChange(bool value)
        {
            _settingDataModel.IsFullScreen = value;
            Screen.fullScreen = value;
            var data = JsonConvert.SerializeObject(_settingDataModel);
            PlayerPrefs.SetString("Setting-Config",data);
            
        }

        private void OnAppear()
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