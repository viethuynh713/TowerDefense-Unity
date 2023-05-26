using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MythicEmpire.InGame
{
    public class TowerUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetElementPosition(Vector3 towerPos)
        {
            RectTransform canvasRect = GetComponent<RectTransform>();
            Vector2 viewportPosition = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToViewportPoint(towerPos);
            Vector2 worldObjectScreenPosition = new Vector2(
                ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
            transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = worldObjectScreenPosition;
        }
    }
}
