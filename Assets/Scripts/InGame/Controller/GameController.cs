using MythicEmpire.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire
{
    public class GameController : MonoBehaviour
    {
        private const int nPlayer = 2;
        private readonly List<Vector3> mapPosList = new List<Vector3>() { new Vector3(1, 0, 0), new Vector3(12, 0, 0) };

        private string gameID;
        private List<GameObject> playerList;
        private GameState state;

        [SerializeField] private GameObject playerController;
        // Start is called before the first frame update
        void Start()
        {
            playerList = new List<GameObject>();
            for (int i = 0; i < nPlayer; i++)
            {
                GameObject player = Instantiate(playerController, mapPosList[i], Quaternion.identity);
                playerList.Add(player);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}