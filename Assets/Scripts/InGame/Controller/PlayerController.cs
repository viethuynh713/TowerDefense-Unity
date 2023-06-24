using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

using MythicEmpire.CommonScript;
using TMPro;
using MythicEmpire.Enums;

namespace MythicEmpire.InGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject hpText;
        [SerializeField] private Slider energySlider;

        private string playerID;
        private int hp;
        private int energy;
        // private List<GameObject> cardList;
        private bool isMyPlayer;
        private Vector2Int monsterGatePos;

        [SerializeField] private GameObject tower;
        [SerializeField] private GameObject monster;
        // Start is called before the first frame update
        void Start()
        {
            playerID = UnityEngine.Random.Range(10000000, 99999999).ToString(); // temp id for testing
            hp = InGameService.playerHp;
            energy = InGameService.playerEnergy;

            SetHPText();
            energySlider.maxValue = InGameService.maxEnergy;
            SetEnergySlider();
        }

        public void Init(bool isMyPlayer)
        {
            this.isMyPlayer = isMyPlayer;
            monsterGatePos = InGameService.monsterGateLogicPos;
            
            RectTransform rt = hpText.GetComponent<RectTransform>();
            if (isMyPlayer)
            {
                rt.anchorMin = new Vector2(1, 0);
                rt.anchorMax = new Vector2(1, 0);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(-200, 100);
                hpText.GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 255);
            }
            else
            {
                rt.anchorMin = new Vector2(1, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(-200, -100);
                hpText.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            if (!isMyPlayer)
            {
                energySlider.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BuildTower(string id, Vector3 displayPos)
        {
            int cost = InGameService.cardCost[new Tuple<CardType, string>(CardType.TowerCard, id)];
            if (energy >= cost)
            {
                SetEnergySlider();
                Vector2Int logicPos = InGameService.Display2LogicPos(displayPos);
                if (GameController.Instance.Map.GetComponent<MapService>().BuildTower(
                    logicPos, isMyPlayer, tower.GetComponent<Tower>()))
                {
                    energy -= cost;
                    SetEnergySlider();
                    GameObject t = Instantiate(tower, InGameService.Logic2DisplayPos(logicPos) + new Vector3(0, 0.16f, 0), Quaternion.identity);
                    t.GetComponent<Tower>().Init(id, isMyPlayer, logicPos);
                }
            }
        }

        public IEnumerator GenerateMonster(string id, int nMonster)
        {
            yield return GenerateMonster(id, nMonster, nMonster);
        }

        public void TakeDmg(int dmg)
        {
            hp -= dmg;
            SetHPText();
            if (hp <= 0)
            {
                GameController.Instance.EndGame();
            }
        }

        public IEnumerator GenerateMonster(string id, int nRestMonster, int nMonster)
        {
            GameObject monsterObj = Instantiate(monster, InGameService.Logic2DisplayPos(monsterGatePos), Quaternion.identity);
            monsterObj.GetComponent<Monster>().Init(playerID, id, !isMyPlayer);
            nRestMonster--;
            if (nRestMonster > 0)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(GenerateMonster(id, nRestMonster, nMonster));
            }
        }

        public void SetHPText()
        {
            hpText.GetComponent<TextMeshProUGUI>().text = "HP: " + hp.ToString();
        }

        public void SetEnergySlider()
        {
            energySlider.value = energy;
        }

        public void GainEnergy(int energyGain)
        {
            energy += energyGain;
            SetEnergySlider();
        }

        public string PlayerId { get { return playerID; } }
        public int Hp { get { return hp; } set { hp = value; SetHPText(); } }
        public int Energy { get { return energy; } set { energy = value; SetEnergySlider(); } }
        public bool IsMyPlayer { get { return isMyPlayer; } }
    }
}