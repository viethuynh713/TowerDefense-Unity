using System.Collections;
using UnityEngine;
using MythicEmpire.Card;

namespace MythicEmpire.InGame
{
    public class Tower : MonoBehaviour
    {
        protected string _towerID;
        protected string _ownerId;
        // protected TowerStats stats;
        protected bool canFire;

        protected int damage;
        protected float attackSpeed;
        protected float fireRange;
        protected float exploreRange;
        protected float bulletSpeed;

        protected float _speedRate =1;
        [SerializeField] protected TowerUI canvas;
        [SerializeField] protected TowerAnimation animation;
        [SerializeField]private Transform rangeUI;
        
        public void Init(string towerId, string ownerId, TowerStats stats)
        {
            _towerID = towerId;
            this._ownerId = ownerId;
            // this.stats = stats;
            
            damage = stats.Damage;
            attackSpeed = stats.AttackSpeed;
            fireRange = stats.FireRange;
            exploreRange = stats.ExploreRange;
            bulletSpeed = stats.BulletSpeed;

            canFire = true;
            
            canvas.SetElementPosition(_towerID, transform.position);
            canvas.gameObject.SetActive(false);
            var scale = ((fireRange + 1) * 3.5f) / transform.localScale.x;
            rangeUI.localScale = new Vector3(scale, rangeUI.localScale.y, scale);
            rangeUI.gameObject.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            Fire();
            
            if (Input.GetMouseButtonDown(1))
            {
                canvas.gameObject.SetActive(false);
                rangeUI.gameObject.SetActive(false);

            }
        }

        protected virtual void Fire()
        {

        }
        
        public void Sell()
        {
            canvas.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        protected IEnumerator LoadBullet()
        {
            canFire = false;
            yield return new WaitForSeconds(1 / (attackSpeed*_speedRate));
            canFire = true;
        }

        public void ActiveUI()
        {
            rangeUI.gameObject.SetActive(true);

            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }

        public string TowerID => _towerID;
        public string OwnerId => _ownerId;

        public void Upgrade(UpgradeTowerDataSender upgradeTowerDataSender)
        {
            damage = upgradeTowerDataSender.damage;
            attackSpeed = upgradeTowerDataSender.attackSpeed;
            fireRange = upgradeTowerDataSender.range;
            var scale = ((fireRange + 1) * 3.5f) / transform.localScale.x;
            rangeUI.localScale = new Vector3(scale, rangeUI.localScale.y, scale);
            canvas.gameObject.SetActive(false);
            rangeUI.gameObject.SetActive(false);


        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,fireRange);
        }

        public void Speedup(float rate)
        {
            if ((rate > 0 && _speedRate >1)||(rate < 0 && _speedRate <1))
            {
                _speedRate = 1 + rate / 100;

            }
            else
            {
                _speedRate += rate / 100;

            }
            _speedRate = 1 + rate / 100;
            StartCoroutine(ResetSpeedupRate());
        }
        IEnumerator  ResetSpeedupRate()
        {
            yield return new WaitForSeconds(0.99f);
            _speedRate = 1;
        }
    }
}
