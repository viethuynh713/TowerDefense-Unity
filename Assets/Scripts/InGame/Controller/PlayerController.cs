using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string playerID;
    private List<GameObject> cardList;
    private GameObject map;
    private bool isMine;
    private GameObject monsterGate;

    [SerializeField] private GameObject monster;
    // Start is called before the first frame update
    void Start()
    {
        // check this 'player' is 'me' or 'opponent'
        if (transform.position.x > 11)
        {
            isMine = true;
        }
        else
        {
            isMine = false;
        }
        // get monster gate
        monsterGate = GameObject.Find("MonsterGate");
        // generate monster (test)
        GenerateMonster();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMonster()
    {
        Instantiate(monster, monsterGate.transform.position, Quaternion.identity);
    }

    public bool IsMine { get { return isMine; } }
}
