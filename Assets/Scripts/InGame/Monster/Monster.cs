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
        private string id;
        private string ownerId;
        private bool isMyPlayer;
        private MonsterStats stats;
        private bool isSummonedByPlayer;
        private bool canAction;
        private bool canAttack;
        private bool isDie;
        private bool isOnPath;
        //private List<Effect> state;
        private List<Vector2Int> path;

        private Animator anim;

        // Start is called before the first frame update
        void Awake()
        {
            path = new List<Vector2Int>();
            stats = new MonsterStats();

            stats.Hp = 10;
            stats.AttackSpeed = 1.5f;
            stats.MoveSpeed = 1;
            stats.AttackRange = 2;
            stats.Damage = 1;

            canAttack = true;
            isSummonedByPlayer = false;
            isDie = false;
            isOnPath = true;

            anim = GetComponent<Animator>();
        }

        public void Init(string ownerId, string id, bool isMyPlayer)
        {
            this.ownerId = ownerId;
            this.id = id;
            this.isMyPlayer = isMyPlayer;
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
                anim.Play("MoveFWD_Normal_InPlace_SwordAndShield");
                if (!isOnPath)
                {
                    isOnPath = true;
                    FindPath();
                }
                Vector3 displayPos = InGameService.Logic2DisplayPos(path[0]);
                transform.LookAt(displayPos);
                transform.position = Vector3.MoveTowards(transform.position, displayPos, stats.MoveSpeed * Time.deltaTime);
                if ((displayPos - transform.position).magnitude < InGameService.infinitesimal)
                {
                    path.RemoveAt(0);
                }
            }
            else
            {
                AttackHouse();
            }
        }

        public void AttackMonster(Transform target)
        {
            if (canAttack)
            {
                anim.Play("Attack01_SwordAndShiled");
                isOnPath = false;
                transform.LookAt(target.transform.position);
                target.gameObject.GetComponent<Monster>().TakeDmg(stats.Damage);
                canAttack = false;
                StartCoroutine(AttackCD());
            }
        }

        public void AttackHouse()
        {
            anim.Play("Attack01_SwordAndShiled");
            isOnPath = true;
            GameController.Instance.GetPlayer(!isMyPlayer).GetComponent<PlayerController>().TakeDmg(stats.Damage);
            Destroy(gameObject);
        }

        public void TakeDmg(int dmg)
        {
            if (!isDie)
            {
                stats.Hp -= dmg;
                if (stats.Hp <= 0)
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
            GameController.Instance.GainEnergy(1, !isMyPlayer);
            anim.Play("Die01_SwordAndShield");
            StartCoroutine(DieModel());
        }

        public void FindPath(Vector2Int? barrierPos = null)
        {
            if (!isDie)
            {
                if (barrierPos == null || path.Contains(barrierPos.Value))
                {
                    path = InGameService.FindPath(
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
            yield return new WaitForSeconds(1 / stats.AttackSpeed);
            canAttack = true;
        }

        private IEnumerator DieModel()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        public bool IsMyPlayer { get { return isMyPlayer; } }
        public MonsterStats Stats { get { return stats; } }
        public bool IsSummonedByPlayer { get { return isSummonedByPlayer; } }
        public bool IsDie { get { return isDie; } }
    }
}
