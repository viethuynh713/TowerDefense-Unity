using MythicEmpire.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using MythicEmpire.Card;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Networking.Model;
using Networking_System.Model;
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
        private bool _isMyPlayer;

        private bool _canAction;

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
            _monsterUI.gameObject.SetActive(false);
        }

        private void Start()
        {
            _monsterAnimation = GetComponent<MonsterAnimation>();
            EventManager.Instance.RegisterListener(EventID.UpdateMonsterHp, HandleUpdateHp);
            EventManager.Instance.RegisterListener(EventID.KillMonster, HandleKilledMonster);
            EventManager.Instance.RegisterListener(EventID.BuildTower, FindNewPathAfterBuildTower);
            EventManager.Instance.RegisterListener(EventID.SellTower, FindNewPathAfterSellTower);
            // StartCoroutine(UpdatePosition());
            previousPosition = new Vector2(transform.position.x, transform.position.z);

        }

        private UpdateMonsterPositionData _positionData = new UpdateMonsterPositionData();
        public Vector2 previousPosition;
        IEnumerator UpdatePosition()
        {
            while (true)
            {
                
                yield return new WaitForSeconds(4f);
                var currentPosition = new Vector2(transform.position.x, transform.position.z);
                if (Vector2.Distance(currentPosition, previousPosition) > 2f)
                {
                    _positionData.monsterId = _id;
                    _positionData.ownerId = _ownerId;
                    _positionData.Xposition = transform.position.x;
                    _positionData.YPosition = transform.position.z;

                    PlayerController_v2.Instance.UpdateMonsterPosition(_positionData);
                    previousPosition = currentPosition;
                }
            }

        }
        private void FindNewPathAfterBuildTower(object obj)
        {
            var data = (TowerModel)obj;
            GameController_v2.Instance.mainThreadAction.Add(() =>
            {
                FindPath(new Vector2Int(data.XLogicPosition,data.YLogicPosition));
                // Debug.Log("Find new path");
                
            });
        }

        private void FindNewPathAfterSellTower(object obj)
        {
            GameController_v2.Instance.mainThreadAction.Add(() =>
            {
                FindPath();
                // Debug.Log("Find new path");

            });
        }
        
        public void HandleUpdateHp(object o)
        {
            GameController_v2.Instance.mainThreadAction.Add(() =>
            {
                var data = (UpdateMonsterHpDataSender)o;
                isSend = true;
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
                    _monsterUI.UpdateMonsterHp(_maxHp, 0);

                    Die();
                }
            });
        }

        private void OnDestroy()
        {
            try
            {
                EventManager.Instance.RemoveListener(EventID.UpdateMonsterHp,HandleUpdateHp);
                EventManager.Instance.RemoveListener(EventID.KillMonster, HandleKilledMonster);
                EventManager.Instance.RemoveListener(EventID.BuildTower,FindNewPathAfterBuildTower);
                EventManager.Instance.RemoveListener(EventID.SellTower,FindNewPathAfterSellTower);

            }
            catch (Exception e)
            {

            }
        }

        public void Init(string monsterId, string ownerId, bool isSummonedByPlayer, MonsterStats stats, int hp, bool isMyMonster)
        {
            _monsterStats = stats;
            _canAction = true;
            _canAttack = true;
            _isDie = false;
            _isOnPath = true;
            _speedupRate = 1f;
            _isMyPlayer = isMyMonster;

            _monsterUI.gameObject.SetActive(true);
            StartCoroutine(UpdatePosition());
            // StartCoroutine(SendUpdateMonsterHp());
            _maxHp = hp;
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
            _canAction = false;
            _isOnPath = true;
            CastleTakeDamageData data = new CastleTakeDamageData()
            {
                HpLose = 1,
                indexPackage = indexPackageUpdateCastle,
                monsterId = _id,
                ownerId = _ownerId,
            };
            PlayerController_v2.Instance.CastleTakeDamage(data);
        }

        public void Heal(int hp)
        {
            MonsterTakeDamageData data = new MonsterTakeDamageData()
            {
                damage = hp,
                indexPackage = indexPackageUpdateHp,
                monsterId = _id,
                ownerId = _ownerId,
            };
            PlayerController_v2.Instance.UpdateMonsterHp(data);

        }

        private int _hpLose = 0;
        private bool isSend = true;
        public void TakeDamage(int dmg)
        {
            _hpLose += dmg;
            _hp -= dmg;
            if(_isDie)return;
            if(!isSend)return;
            if ( _hp < 50)
            {
                MonsterTakeDamageData data = new MonsterTakeDamageData()
                {
                    damage = -_hpLose,
                    indexPackage = indexPackageUpdateHp,
                    monsterId = _id,
                    ownerId = _ownerId,
                };
                _hpLose = 0;
                isSend = false;
                PlayerController_v2.Instance.UpdateMonsterHp(data);
            }
            else
            {
                _monsterUI.UpdateMonsterHp(_maxHp,_hp);

            }
            
        }



        public void Speedup(float rateup)
        {
            if (!_isDie)
            {
                _speedupRate = 1 + rateup/100;
                StartCoroutine(ResetSpeedupRate());
            }
        }
        IEnumerator  ResetSpeedupRate()
        {
            yield return new WaitForSeconds(0.49f);
            _speedupRate = 1;
        }

        private void Die()
        {
            if (!IsDie)
            {
                _monsterAnimation.PlayAnimation("die");
                _isDie = true;
                Destroy(gameObject, 3);
            }
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
                        GameController_v2.Instance.mapService.GetRivalCastlePosition(_ownerId),
                        _isMyPlayer);
                    
                }
            }
        }

        private IEnumerator AttackCD()
        {
            yield return new WaitForSeconds(1 / (_attackSpeed*_speedupRate));
            _canAttack = true;
        }

        public string Id { get { return _id; } }
        public string OwnerId { get { return _ownerId; } }
        public float AttackRange { get { return _attackRange; } }
        public bool IsSummonedByPlayer { get { return _isSummonedByPlayer; } }
        public bool IsDie { get { return _isDie; } }
        public int Cost { get { return _monsterStats.Energy; } }
        public bool CanAction { get { return _canAction; } set { _canAction = value; } }

        public void View()
        {
            _monsterUI.gameObject.SetActive(false);
        }
    }
}
