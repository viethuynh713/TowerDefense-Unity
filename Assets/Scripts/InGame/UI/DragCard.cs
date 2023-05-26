using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MythicEmpire.Enums;

namespace MythicEmpire.InGame
{
    public class DragCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image dragIcon;

        [SerializeField] private CardType type;
        [SerializeField] private string id;
        private Vector2 originAnchoredPos;
        private CanvasGroup canvasGroup;

        void Start()
        {
            originAnchoredPos = dragIcon.GetComponent<RectTransform>().anchoredPosition;
            canvasGroup = dragIcon.GetComponent<CanvasGroup>();
            dragIcon.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragIcon.gameObject.SetActive(true);
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragIcon.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // blur icon
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            // build tower
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycast))
            {
                GameController.Instance.BuildTower(id, raycast.point, true);
            }
            // reset icon
            dragIcon.gameObject.SetActive(false);
            dragIcon.GetComponent<RectTransform>().anchoredPosition = originAnchoredPos;
        }
    }
}
