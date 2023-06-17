using MythicEmpire.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using MythicEmpire.Card;

namespace MythicEmpire.InGame
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private string id;
        private string ownerId;
        private bool isMyPlayer;
        [SerializeField] private MonsterStats stats;
        private bool isSummonedByPlayer;
        private bool canAction;
        private bool canAttack;
        private bool isDie;
        private bool isOnPath;
        //private List<Effect> state;
        private List<Vector2Int> path;

        private int hp;
        private float attackSpeed;
        private float moveSpeed;
        private float attackRange;
        private int damage;

        // Start is called before the first frame update
        void Awake()
        {
            path = new List<Vector2Int>();

            canAttack = true;
            isDie = false;
            isOnPath = true;

            hp = stats.Hp;
            attackSpeed = stats.AttackSpeed;
            moveSpeed = stats.MoveSpeed;
            attackRange = stats.AttackRange;
            damage = stats.Damage;
        }

        public void Init(string ownerId, bool isMyPlayer, bool isSummonedByPlayer)
        {
            this.ownerId = ownerId;
            this.isMyPlayer = isMyPlayer;
            this.isSummonedByPlayer = isSummonedByPlayer;
            FindPath();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Move()
        {
            if (path.Count > 0)
            {
                GetComponent<MonsterAnimation>().PlayAnimation("move");
                if (!isOnPath)
                {
                    isOnPath = true;
                    FindPath();
                }
                Vector3 displayPos = InGameService.Logic2DisplayPos(path[0]);
                transform.LookAt(displayPos);
                transform.position = Vector3.MoveTowards(transform.position, displayPos, moveSpeed * Time.deltaTime);
                if ((displayPos - transform.position).magnitude < InGameService.infinitesimal)
                {
                    path.RemoveAt(0);
                    if (path.Count == 0)
                    {
                        AttackHouse();
                    }
                }
            }
        }

        public void AttackMonster(Transform target)
        {
            if (canAttack)
            {
                GetComponent<MonsterAnimation>().PlayAnimation("attack");
                isOnPath = false;
                transform.LookAt(target.transform.position);
                target.gameObject.GetComponent<Monster>().TakeDmg(damage);
                canAttack = false;
                StartCoroutine(AttackCD());
            }
        }

        public void AttackHouse()
        {
            GetComponent<MonsterAnimation>().PlayAnimation("attack");
            isOnPath = true;
            GameController.Instance.GetPlayer(!isMyPlayer).GetComponent<PlayerController>().TakeDmg(damage);
            Destroy(gameObject);
        }

        public void TakeDmg(int dmg)
        {
            if (!isDie)
            {
                hp -= dmg;
                if (hp <= 0)
                {
                    Die();
                }
            }
        }

        public void AddEffect(Effect effect)
        {

        }

        public void ExecuteEffect()
        {

        }

        public void Die()
        {
            isDie = true;
            GameController.Instance.GainEnergy(stats.EnergyGainWhenDie, !isMyPlayer);
            GetComponent<MonsterAnimation>().PlayAnimation("die");
            StartCoroutine(DieModel());
        }

        public void FindPath(Vector2Int? barrierPos = null)
        {
            if (!isDie)
            {
                if (barrierPos == null || path.Contains(barrierPos.Value))
                {
                    path = InGameService.FindPathForMonster(
                        GameController.Instance.Map.GetComponent<MapService>().CurrentMap,
                        InGameService.Display2LogicPos(transform.position),
                        InGameService.houseLogicPos[isMyPlayer ? TypePlayer.Opponent : TypePlayer.Player],
                        !isMyPlayer
                    );
                }
            }
        }

        private IEnumerator AttackCD()
        {
            yield return new WaitForSeconds(1 / attackSpeed);
            canAttack = true;
        }

        private IEnumerator DieModel()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        public string Id { get { return id; } }
        public bool IsMyPlayer { get { return isMyPlayer; } }
        public float AttackRange { get { return attackRange; } }
        public bool IsSummonedByPlayer { get { return isSummonedByPlayer; } }
        public bool IsDie { get { return isDie; } }
        public int Cost { get { return stats.Energy; } }
    }
}
