// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System;
// using UnityEngine;
// using UnityEngine.UI;
//
// using MythicEmpire.CommonScript;
// using TMPro;
// using MythicEmpire.Enums;
// using MythicEmpire.Manager.MythicEmpire.Manager;
// using MythicEmpire.Card;
//
// namespace MythicEmpire.InGame
// {
//     public class PlayerController : MonoBehaviour
//     {
//         [SerializeField] private GameObject hpText;
//         [SerializeField] private Slider energySlider;
//
//         private string playerID;
//         private int hp;
//         private int energy;
//         // private List<GameObject> cardList;
//         private bool isMyPlayer;
//         private Vector2Int monsterGatePos;
//         // Start is called before the first frame update
//         void Start()
//         {
//             playerID = UnityEngine.Random.Range(10000000, 99999999).ToString(); // temp id for testing
//             hp = InGameService.playerHp;
//             energy = InGameService.playerEnergy;
//
//             SetHPText();
//             energySlider.maxValue = InGameService.maxEnergy;
//             SetEnergySlider();
//
//             //EventManager.Instance.RegisterListener(EventID.BuildTower, BuildTower);
//         }
//
//         public void Init(bool isMyPlayer)
//         {
//             this.isMyPlayer = isMyPlayer;
//             monsterGatePos = InGameService.monsterGateLogicPos;
//             
//             RectTransform rt = hpText.GetComponent<RectTransform>();
//             if (isMyPlayer)
//             {
//                 rt.anchorMin = new Vector2(1, 0);
//                 rt.anchorMax = new Vector2(1, 0);
//                 rt.pivot = new Vector2(0.5f, 0.5f);
//                 rt.anchoredPosition = new Vector2(-200, 100);
//                 hpText.GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 255);
//             }
//             else
//             {
//                 rt.anchorMin = new Vector2(1, 1);
//                 rt.anchorMax = new Vector2(1, 1);
//                 rt.pivot = new Vector2(0.5f, 0.5f);
//                 rt.anchoredPosition = new Vector2(-200, -100);
//                 hpText.GetComponent<TextMeshProUGUI>().color = Color.red;
//             }
//
//             if (!isMyPlayer)
//             {
//                 energySlider.gameObject.SetActive(false);
//             }
//         }
//
//         // Update is called once per frame
//         void Update()
//         {
//
//         }
//
//         // use to test in client
//         public void BuildTower(string id, Vector3 displayPos)
//         {
//             // get tower by id and check if having enough energy to build
//             GameObject tower = GameController.Instance.GetComponent<TowerFactory>().GetTower(id);
//             int cost = tower.GetComponent<Tower>().Cost;
//             if (energy >= cost)
//             {
//                 // check if building tower is valid
//                 Vector2Int logicPos = InGameService.Display2LogicPos(displayPos);
//                 if (GameController.Instance.Map.GetComponent<MapService>().BuildTower(
//                     logicPos, isMyPlayer, tower.GetComponent<Tower>()))
//                 {
//                     // cost energy and build tower
//                     energy -= cost;
//                     SetEnergySlider();
//                     GameObject t = Instantiate(tower, InGameService.Logic2DisplayPos(logicPos) + new Vector3(0, 0.16f, 0), Quaternion.identity);
//                     // t.GetComponent<Tower>().Init(isMyPlayer, logicPos);
//                 }
//             }
//         }
//
//         // use to communicate with server
//         public void BuildTower(object _cardData)
//         {
//             //var cardData = (CardInfo)_cardData;
//             //int cost = cardData.CardStats.Energy;
//             //if (energy >= cost)
//             //{
//             //    SetEnergySlider();
//             //    Vector2Int logicPos = InGameService.Display2LogicPos(displayPos);
//             //    if (GameController.Instance.Map.GetComponent<MapService>().BuildTower(
//             //        logicPos, isMyPlayer, tower.GetComponent<Tower>()))
//             //    {
//             //        energy -= cost;
//             //        SetEnergySlider();
//             //        GameObject t = Instantiate(tower, InGameService.Logic2DisplayPos(logicPos) + new Vector3(0, 0.16f, 0), Quaternion.identity);
//             //        t.GetComponent<Tower>().Init(cardData.CardId, isMyPlayer, logicPos);
//             //    }
//             //}
//         }
//
//         public IEnumerator GenerateMonsterAuto(string id, int nMonster)
//         {
//             yield return GenerateMonster(id, nMonster, nMonster, false);
//         }
//
//         public void GenerateMonsterByPlayer(string id, Vector3 displayPos)
//         {
//             // get monster by id and check if having enough energy to generate
//             GameObject monster = GameController.Instance.GetComponent<MonsterFactory>().GetMonster(id);
//             int cost = monster.GetComponent<Monster>().Cost;
//             if (energy >= cost)
//             {
//                 // check if generating is valid
//                 Vector2Int logicPos = InGameService.Display2LogicPos(displayPos);
//                 if (GameController.Instance.Map.GetComponent<MapService>().IsGenMonsterValid(logicPos, isMyPlayer))
//                 {
//                     // cost energy and generate monster
//                     energy -= cost;
//                     SetEnergySlider();
//                     GameObject monsterObj = Instantiate(monster, InGameService.Logic2DisplayPos(logicPos), Quaternion.identity);
//                     // monsterObj.GetComponent<Monster>().Init(playerID, isMyPlayer, true);
//                 }
//             }
//         }
//
//         public void UseSpell(string id, Vector3 displayPos)
//         {
//             // get spell by id and check if having enough energy to use
//             GameObject spell = GameController.Instance.GetComponent<SpellFactory>().GetSpell(id);
//             int cost = spell.GetComponent<Spell>().Cost;
//             if (energy >= cost)
//             {
//                 // check if using spell is valid
//                 if (GameController.Instance.Map.GetComponent<MapService>().IsGenSpellValid(displayPos, isMyPlayer))
//                 {
//                     // cost energy and use spell
//                     energy -= cost;
//                     SetEnergySlider();
//                     GameObject spellObj = Instantiate(spell, displayPos, Quaternion.identity);
//                     // spellObj.GetComponent<Spell>().Init(playerID, isMyPlayer);
//                 }
//             }
//         }
//
//         public void TakeDmg(int dmg)
//         {
//             hp -= dmg;
//             SetHPText();
//             if (hp <= 0)
//             {
//                 GameController.Instance.EndGame();
//             }
//         }
//
//         public IEnumerator GenerateMonster(string id, int nRestMonster, int nMonster, bool isSummonedByPlayer = false)
//         {
//             GameObject monster = GameController.Instance.GetComponent<MonsterFactory>().GetMonster(id);
//             GameObject monsterObj = Instantiate(monster, InGameService.Logic2DisplayPos(monsterGatePos), Quaternion.identity);
//             // monsterObj.GetComponent<Monster>().Init(playerID, !isMyPlayer, false);
//             nRestMonster--;
//             if (nRestMonster > 0)
//             {
//                 yield return new WaitForSeconds(1);
//                 StartCoroutine(GenerateMonster(id, nRestMonster, nMonster, isSummonedByPlayer));
//             }
//         }
//
//         public void SetHPText()
//         {
//             hpText.GetComponent<TextMeshProUGUI>().text = "HP: " + hp.ToString();
//         }
//
//         public void SetEnergySlider()
//         {
//             energySlider.value = energy;
//         }
//
//         public void GainEnergy(int energyGain)
//         {
//             energy += energyGain;
//             SetEnergySlider();
//         }
//
//         public string PlayerId { get { return playerID; } }
//         public int Hp { get { return hp; } set { hp = value; SetHPText(); } }
//         public int Energy { get { return energy; } set { energy = value; SetEnergySlider(); } }
//         public bool IsMyPlayer { get { return isMyPlayer; } }
//     }
// }