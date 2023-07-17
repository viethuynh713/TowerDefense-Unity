using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MythicEmpire.InGame
{
    public class TowerUI : MonoBehaviour
    {
        [SerializeField] private Image UI;
        private string _towerId;
        public void SetElementPosition(string id, Vector3 towerPos)
        {
            _towerId = id;
            _upgradeTowerData.towerId = id;
            RectTransform canvasRect = GetComponent<RectTransform>();
            Vector2 viewportPosition = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToViewportPoint(towerPos);
            Vector2 worldObjectScreenPosition = new Vector2(
                ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
            UI.gameObject.GetComponent<RectTransform>().anchoredPosition = worldObjectScreenPosition;
        }

        public void SellTower()
        {
            SellTowerData data = new SellTowerData()
            {
                towerId = _towerId
            };
            PlayerController_v2.Instance.SellTower(data);
        }

        private UpgradeTowerData _upgradeTowerData = new UpgradeTowerData();
        public void UpgradeDamage()
        {
            _upgradeTowerData.type = UpgradeType.Damage;
            PlayerController_v2.Instance.UpgradeTower(_upgradeTowerData);
        }
        public void UpgradeRange()
        {
            _upgradeTowerData.type = UpgradeType.Range;
            PlayerController_v2.Instance.UpgradeTower(_upgradeTowerData);


        }
        public void UpgradeSpeed()
        {
            _upgradeTowerData.type = UpgradeType.AttackSpeed;
            PlayerController_v2.Instance.UpgradeTower(_upgradeTowerData);


        }
    }
}
