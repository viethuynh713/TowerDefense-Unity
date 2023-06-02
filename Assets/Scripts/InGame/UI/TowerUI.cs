using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MythicEmpire.InGame
{
    public class TowerUI : MonoBehaviour
    {
        [SerializeField] private Image UI;
        public void SetElementPosition(Vector3 towerPos)
        {
            RectTransform canvasRect = GetComponent<RectTransform>();
            Vector2 viewportPosition = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToViewportPoint(towerPos);
            Vector2 worldObjectScreenPosition = new Vector2(
                ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
            UI.gameObject.GetComponent<RectTransform>().anchoredPosition = worldObjectScreenPosition;
        }
    }
}
