using MythicEmpire.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        void Start()
        {
            path = new List<Vector2Int>();
            stats = new MonsterStats();

            stats.Hp = 10;
            stats.AttackSpeed = 1.5f;
            stats.MoveSpeed = 1;
            stats.DetectRange = 6;
            stats.AttackRange = 2;
            stats.Damage = 1;

            canAttack = true;
            isSummonedByPlayer = true;
            isDie = false;
            isOnPath = true;

            anim = GetComponent<Animator>();

            FindPath();
        }

        public void Init(string ownerId, bool isMyPlayer)
        {
            this.ownerId = ownerId;
            this.isMyPlayer = isMyPlayer;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MoveToHouse()
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
                transform.Translate(transform.forward * stats.MoveSpeed * Time.deltaTime);
                if (Mathf.Abs((displayPos - transform.position).magnitude) < 0.01f)
                {
                    path.RemoveAt(0);
                }
            }
            else
            {
                AttackHouse();
            }
        }

        public void MoveToMonster(Transform target)
        {
            anim.Play("MoveFWD_Normal_InPlace_SwordAndShield");
            isOnPath = false;
            transform.LookAt(target.transform.position);
            transform.Translate(transform.forward * stats.MoveSpeed * Time.deltaTime);
        }

        public void AttackMonster(Transform target)
        {
            if (canAttack)
            {
                anim.Play("Attack01_SwordAndShiled");
                isOnPath = false;
                target.gameObject.GetComponent<Monster>().TakeDmg(stats.Damage);
                canAttack = false;
                StartCoroutine(AttackCD());
            }
        }

        public void AttackHouse()
        {
            anim.Play("Attack01_SwordAndShiled");
            isOnPath = true;
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
            anim.Play("Die01_SwordAndShield");
            StartCoroutine(DieModel());
        }

        public void FindPath()
        {
            path = InGameService.FindPath(
                FindObjectOfType<GameController>().Map.GetComponent<MapService>().CurrentMap,
                InGameService.Display2LogicPos(transform.position),
                InGameService.houseLogicPos[isMyPlayer ? TypePlayer.Player : TypePlayer.Opponent]
            );
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

        public MonsterStats Stats { get { return stats; } }
        public bool IsSummonedByPlayer { get { return isSummonedByPlayer; } }
        public bool IsDie { get { return isDie; } }
    }
}
