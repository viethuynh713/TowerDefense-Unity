using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire
{
    public class Monster : MonoBehaviour
    {
        private int id;
        private int ownerId;
        private bool canAction;
        private MonsterStats stats;
        //private List<Effect> state;
        private List<Vector2Int> path;

        // Start is called before the first frame update
        void Start()
        {
            path = new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) };
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void Move()
        {

        }

        public void AttackMonster()
        {

        }

        public void AttackHouse()
        {

        }

        public void TakeDmg(int dmg)
        {

        }

        public void AddEffect(Effect effect)
        {

        }

        public void ExecuteEffect()
        {

        }

        public void Die()
        {

        }

        public void FindPath()
        {

        }
    }
}
