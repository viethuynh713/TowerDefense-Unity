using MythicEmpire.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythicEmpire.InGame
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;
        private string gameID;
        private List<GameObject> playerList;
        [SerializeField] private GameObject map;
        private GameState state;
        private int wave;

        [SerializeField] private GameObject playerController;

        [SerializeField] private Slider nextWaveTimeSlider;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // initial 2 playerController (the first in playerList is myPlayer)
            playerList = new List<GameObject>();
            for (int i = 0; i < InGameService.nPlayer; i++)
            {
                GameObject player = Instantiate(
                    playerController, InGameService.Logic2DisplayPos(InGameService.mapLogicPos), Quaternion.identity
                );
                player.GetComponent<PlayerController>().Init(i == 0 ? true : false);
                playerList.Add(player);
            }
            wave = 0;

            nextWaveTimeSlider.maxValue = InGameService.waveTimeDelay;
            nextWaveTimeSlider.value = InGameService.waveTimeDelay;

            StartCoroutine(StartGame());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static GameController Instance { get { return instance; } }

        public GameObject GetPlayer(string id)
        {
            foreach (GameObject player in playerList)
            {
                if (player.GetComponent<PlayerController>().PlayerId == id)
                {
                    return player;
                }
            }
            return null;
        }

        public GameObject GetPlayer(bool isMyPlayer)
        {
            foreach (GameObject player in playerList)
            {
                if (player.GetComponent<PlayerController>().IsMyPlayer == isMyPlayer)
                {
                    return player;
                }
            }
            return null;
        }

        public void EndGame()
        {

        }

        public void BuildTower(string id, Vector3 displayPos, bool isMyPlayer)
        {
            int index = isMyPlayer ? 0 : 1;
            playerList[index].GetComponent<PlayerController>().BuildTower(id, displayPos);
        }

        public void SellTower(Vector2Int logicPos, bool isMyPlayer, int energyGain)
        {
            map.GetComponent<MapService>().SellTower(logicPos);
            int index = isMyPlayer ? 0 : 1;
            playerList[index].GetComponent<PlayerController>().GainEnergy(energyGain);
        }

        public void GenerateMonsterAuto()
        {
            StartCoroutine(WaitForNextMonsterWave(InGameService.waveTimeDelay));
            foreach (GameObject player in playerList)
            {
                foreach (var ele in InGameService.monsterWave[wave])
                {
                    StartCoroutine(player.GetComponent<PlayerController>().GenerateMonster(ele.Item1, ele.Item2));
                }
            }
        }

        public void GainEnergy(int energyGain, bool isMyPlayer)
        {
            int index = isMyPlayer ? 0 : 1;
            playerList[index].GetComponent<PlayerController>().GainEnergy(energyGain);
        }

        private IEnumerator WaitForNextMonsterWave(int time)
        {
            nextWaveTimeSlider.value = time;
            yield return new WaitForSeconds(1);
            time--;
            if (time == 0)
            {
                if (wave < InGameService.nWave - 1)
                {
                    wave++;
                    GenerateMonsterAuto();
                }
                else
                {
                    nextWaveTimeSlider.value = time;
                    Debug.Log("End Game!");
                }
            }
            else
            {
                StartCoroutine(WaitForNextMonsterWave(time));
            }
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(3);
            GenerateMonsterAuto();
        }

        public GameObject Map { get { return map; } }
    }
}