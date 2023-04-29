using MythicEmpire.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class GameController : MonoBehaviour
    {
        private string gameID;
        private List<GameObject> playerList;
        [SerializeField] private GameObject map;
        private GameState state;

        [SerializeField] private GameObject playerController;
        // Start is called before the first frame update
        void Start()
        {
            playerList = new List<GameObject>();
            for (int i = 0; i < InGameService.nPlayer; i++)
            {
                GameObject player = Instantiate(
                    playerController, InGameService.Logic2DisplayPos(InGameService.mapLogicPos), Quaternion.identity
                );
                player.GetComponent<PlayerController>().Init(i == 0 ? true : false);
                playerList.Add(player);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

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

        public GameObject Map { get { return map; } }
    }
}