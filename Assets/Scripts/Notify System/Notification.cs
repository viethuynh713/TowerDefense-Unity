using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MythicEmpire.Manager
{
    public class Notification : MonoBehaviour,IInGameNotify,IPopupNotify
    {
        public static Notification Instance;
        [SerializeField] private RectTransform _statusPanel;
        [SerializeField] private GameObject _popupPanel;
        [SerializeField] private TMP_Text _messageStatusText;
        [SerializeField] private TMP_Text _messagePopupText;
        private Vector2 _statusPanelPos;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
            }

            _statusPanelPos = _statusPanel.anchoredPosition;
            _popupPanel.SetActive(false);

        }

        public void NotifyStatus(string message)
        {
            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(_statusPanel.DOAnchorPosY(_statusPanelPos.y - 100, 0.5f));
            mySequence.Insert(1.5f, _statusPanel.DOAnchorPosY(_statusPanelPos.y, 0.5f));
            _messageStatusText.text =  message;
        }

        public void PopupNotifyError(string message)
        {
            _popupPanel.SetActive(true);
            _messagePopupText.text =  message;
        }

        public void PopupNotifyWaring(string message)
        {
            _popupPanel.SetActive(true);
            _messagePopupText.text = message;

        }
    }
}
