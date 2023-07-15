using MythicEmpire.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using MythicEmpire.Card;
using MythicEmpire.Manager.MythicEmpire.Manager;
using Networking_System.Model;
using Networking_System.Model.ReceiveData;
using Newtonsoft.Json.Linq;

namespace MythicEmpire.InGame
{
    [RequireComponent(typeof(MonsterAnimation))]
    public class Monster : MonoBehaviour
    {
        private string _id;
        private string _ownerId;
        private bool _isSummonedByPlayer;
        private bool _canAttack;
        private bool _isDie;
        private bool _isOnPath;
        private List<Vector2Int> _path;
        private float _speedupRate;

        private bool _canAction;
        private float _notActionTime;

        private int _maxHp;
        private int _hp;
        private float _attackSpeed;
        private float _moveSpeed;
        private float _attackRange;
        private int _damage;
        private MonsterStats _monsterStats;
        private MonsterAnimation _monsterAnimation;
        private int indexPackageUpdateHp;
        private int indexPackageUpdateCastle;
        [SerializeField] private MonsterUI _monsterUI;

        private void Awake()
        {
            _path = new List<Vector2Int>();
            _canAction = false;
            _canAttack = false;
            _isOnPath = false;

        }

        private void Start()
        {
            _monsterAnimation = GetComponent<MonsterAnimation>();
            EventManager.Instance.RegisterListener(EventID.UpdateMonsterHp, HandleUpdateHp);
            EventManager.Instance.RegisterListener(EventID.KillMonster, HandleKilledMonster);
            // EventManager.Instance.RegisterListener(EventID.UpdateCastleHp, HandleUpdateCastleHp);
            
        }

        // private void HandleUpdateCastleHp(object data)
        // {
        //     GameController_v2.Instance.mainThreadAction.Add(() =>
        //     {
        //         var castleData = (CastleTakeDamageSender)data;
        //         if (castleData.userId == _ownerId)
        //         {
        //             indexPackageUpdateCastle = castleData.indexPackage;
        //             Debug.Log($"... {indexPackageUpdateCastle}=>{castleData.indexPackage}");
        //         }
        //     });
        // }

        public void HandleUpdateHp(object o)
        {
            GameController_v2.Instance.mainThreadAction.Add(() =>
            {
                var data = (UpdateMonsterHpDataSender)o;
                if (data.monsterId == _id)
                {
                    if(data.indexPackage <= indexPackageUpdateHp)return;
                    indexPackageUpdateHp = data.indexPackage;
                    _hp = data.currentHp;
                    _monsterUI.UpdateMonsterHp(_maxHp, _hp);
                }
            });
        }

        public void HandleKilledMonster(object o)
        {
            GameController_v2.Instance.mainThreadAction.Add(()=>
            {
                if (_id == (string)o)
                {
                    Die();
                }
            });
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(EventID.UpdateMonsterHp,HandleUpdateHp);
            EventManager.Instance.RemoveListener(EventID.KillMonster, HandleKilledMonster);
        }

        public void Init(string monsterId, string ownerId, bool isSummonedByPlayer, MonsterStats stats, bool isMyMonster)
        {
            _monsterStats = stats;
            _canAction = true;
            _canAttack = true;
            _isDie = false;
            _isOnPath = true;
            _speedupRate = 1f;
            _notActionTime = 0f;

            _maxHp = stats.Hp;
            _hp = _maxHp;
            _attackSpeed = stats.AttackSpeed;
            _moveSpeed = stats.MoveSpeed;
            _attackRange = stats.AttackRange;
            _damage = stats.Damage;
            
            _ownerId = ownerId;
            _id = monsterId;
            _isSummonedByPlayer = isSummonedByPlayer;
            _monsterUI.Init(_maxHp,isMyMonster);
            indexPackageUpdateHp = 0;
            indexPackageUpdateCastle = 0;
            FindPath();
        }

        public void Move()
        {
            if (_canAction)
            {
                if (_path.Count > 0)
                {
                    _monsterAnimation.PlayAnimation("move");
                    if (!_isOnPath)
                    {
                        _isOnPath = true;
                        FindPath();
                    }
                    Vector3 displayPos = InGameService.Logic2DisplayPos(_path[0]);
                    transform.LookAt(displayPos);
                    transform.position = Vector3.MoveTowards(transform.position, displayPos, _moveSpeed * _speedupRate * Time.deltaTime);
                    _speedupRate = 1f;
                    if ((displayPos - transform.position).magnitude < InGameService.infinitesimal)
                    {
                        _path.RemoveAt(0);
                        if (_path.Count == 0)
                        {
                            AttackHouse();
                        }
                    }
                }
            }
        }

        public void AttackMonster(Transform target)
        {
            if (_canAction && _canAttack)
            {
                _monsterAnimation.PlayAnimation("attack");
                _isOnPath = false;
                transform.LookAt(target.transform.position);
                target.gameObject.GetComponent<Monster>().TakeDamage(_damage);
                _canAttack = false;
                StartCoroutine(AttackCD());
            }
        }

        private void AttackHouse()
        {
            _monsterAnimation.PlayAnimation("attack");
            _isOnPath = true;
            CastleTakeDamageData data = new CastleTakeDamageData()
            {
                HpLose = 1,
                indexPackage = indexPackageUpdateCastle,
                monsterId = _id,
                ownerId = _ownerId,
            };
            PlayerController_v2.Instance.CastleTakeDamage(data);
            // gameObject.SetActive(false);
        }

        public void Heal(int hp)
        {
            // if (!_isDie)
            // {
            //     _hp += hp;
            //     if (this._hp > _maxHp)
            //     {
            //         this._hp = _maxHp;
            //     }
            // }
            MonsterTakeDamageData data = new MonsterTakeDamageData()
            {
                damage = hp,
                indexPackage = indexPackageUpdateHp,
                monsterId = _id,
                ownerId = _ownerId,
            };
            PlayerController_v2.Instance.UpdateMonsterHp(data);

        }

        public void TakeDamage(int dmg)
        {
            // if (!_isDie)
            // {
            //     _hp -= dmg;
            //     if (_hp <= 0)
            //     {
            //         Die();
            //     }
            // }
            MonsterTakeDamageData data = new MonsterTakeDamageData()
            {
                damage = -dmg,
                indexPackage = indexPackageUpdateHp,
                monsterId = _id,
                ownerId = _ownerId,
            };
            PlayerController_v2.Instance.UpdateMonsterHp(data);

        }

        public void Freezed(float freezeTime)
        {
            if (!_isDie)
            {
                if (_notActionTime < freezeTime)
                {
                    _notActionTime = freezeTime;
                }
                if (_canAction)
                {
                    StartCoroutine(FreezeCD());
                }
                _canAction = false;
            }
        }

        private IEnumerator FreezeCD()
        {
            yield return new WaitForSeconds(0.1f);
            _notActionTime -= 0.1f;
            if (_notActionTime > 0)
            {
                StartCoroutine(FreezeCD());
            }
            else
            {
                _canAction = true;
            }
        }

        public void Speedup(float rateup)
        {
            if (!_isDie)
            {
                _speedupRate *= rateup;
            }
        }

        public void Die()
        {
            _isDie = true;
            // GameController.Instance.GainEnergy(stats.EnergyGainWhenDie, !isMyPlayer);
            // PlayerController_v2.Instance.GainEnergy(_ownerId, _monsterStats.EnergyGainWhenDie);
            _monsterAnimation.PlayAnimation("die");
            StartCoroutine(DieModel());
        }

        private void FindPath(Vector2Int? barrierPos = null)
        {
            if (!_isDie)
            {
                if (barrierPos == null || _path.Contains(barrierPos.Value))
                {
                    _path = InGameService.FindPathForMonster(
                        GameController_v2.Instance.mapService.CurrentMap,
                        InGameService.Display2LogicPos(transform.position),
                        GameController_v2.Instance.mapService.GetRivalCastlePosition(_ownerId));
                    
                }
            }
        }

        private IEnumerator AttackCD()
        {
            yield return new WaitForSeconds(1 / _attackSpeed);
            _canAttack = true;
        }

        private IEnumerator DieModel()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        public string Id { get { return _id; } }
        public string OwnerId { get { return _ownerId; } }
        public float AttackRange { get { return _attackRange; } }
        public bool IsSummonedByPlayer { get { return _isSummonedByPlayer; } }
        public bool IsDie { get { return _isDie; } }
        public int Cost { get { return _monsterStats.Energy; } }
        public bool CanAction { get { return _canAction; } set { _canAction = value; } }
    }
}
