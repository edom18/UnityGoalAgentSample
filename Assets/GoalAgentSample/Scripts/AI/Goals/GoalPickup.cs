using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// 指定されたアイテムを手に入れるゴール
    ///
    /// 一定距離近づいていたら取得できる
    /// </summary>
    public class GoalPickup<T> : Goal<T> where T : AIBase
    {
        private GameObject _target;
        private float _canGetDistance = 1.0f;

        /// <summary>
        /// 対象オブジェクトが手に入れられる距離にあるかを判定する
        /// </summary>
        bool CanPickup
        {
            get
            {
                var item = _target.GetComponent<Item>();
                if (item == null)
                {
                    return false;
                }

                var distance = Vector3.Distance(_target.transform.position, _owner.transform.position);
                if (distance > _canGetDistance)
                {
                    Debug.Log("Cannot pickup item. Agent has been far distance.");
                    return false;
                }

                return true;
            }
        }
           

        #region Constructor

        // コンストラクタ
        public GoalPickup(T owner, GameObject target) : base(owner)
        {
            _target = target;
        }

        #endregion

        /// <summary>
        /// アイテムを実際にピックアップ
        /// </summary>
        void Pickup()
        {
            var item = _target.GetComponent<Item>();

            switch (item.ItemType)
            {
                case ItemType.Energy:
                    _owner.Energy += 0.1f;
                    break;

                case ItemType.Power:
                    _owner.AttackPower += 0.1f;
                    break;
            }

            item.PickedUp();
        }


        #region Override

        public override void Activate()
        {
            base.Activate();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (CanPickup)
            {
                Pickup();
                _status = Status.Completed;
            }

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Terminated GoalPickup.");
        }

        #endregion
    }
}