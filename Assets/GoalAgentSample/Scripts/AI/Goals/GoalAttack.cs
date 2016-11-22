using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// ターゲットを攻撃するゴール
    /// 
    /// 距離制限外の場合は失敗する
    /// </summary>
    public class GoalAttack<T> : Goal<T> where T : AIBase
    {
        private GameObject _targetObj;
        private Vector3 _targetPos;
        private float _canAttackDistance = 0.3f;

        /// <summary>
        /// 対象オブジェクトに攻撃できるか
        /// </summary>
        bool CanAttack
        {
            get
            {
                var distance = Vector3.Distance(_targetPos, _owner.transform.position);
                if (distance > _canAttackDistance)
                {
                    return false;
                }

                if (_owner.AttackPower < 0.1f)
                {
                    return false;
                }

                return true;
            }
        }
           

        #region コンストラクタ

        public GoalAttack(T owner, GameObject targetObj, Vector3 targetPos) : base(owner)
        {
            _targetObj = targetObj;
            _targetPos = targetPos;
        }

        #endregion


        /// <summary>
        /// アイテムを実際にピックアップ
        /// </summary>
        void Attack()
        {
            Debug.Log("ATTACK!!");

            _owner.AttackPower -= 0.1f;

            var bulletObj = GameObject.Instantiate(_owner.BulletPrefab);
            bulletObj.transform.position = _owner.transform.position + _owner.transform.forward * 0.4f;

            var bullet = bulletObj.GetComponent<Bullet>();
            var dir = _targetObj.transform.position - bulletObj.transform.position;
            bullet.Shot(dir, 5f);
        }


        #region Override goal methods.

        public override Status Process()
        {
            ActivateIfInactive();

            if (CanAttack)
            {
                Attack();
                _status = Status.Completed;
            }
            else
            {
                Debug.Log("Cannot attack target.");
                _status = Status.Failed;
            }

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Terminated GoalAttack.");
        }

        #endregion
    }
}