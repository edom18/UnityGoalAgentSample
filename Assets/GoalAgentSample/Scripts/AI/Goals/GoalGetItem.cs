using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// アイテムを取得するためのゴール
    /// 取得できるアイテムは実際にオブジェクトに近づいたときにオブジェクトから状態を得る
    /// </summary>
    public class GoalGetItem<T> : CompositeGoal<T> where T : AIBase
    {
        private GameObject _target;

        #region Constructor

        // コンストラクタ
        public GoalGetItem(T owner, GameObject target) : base(owner)
        {
            _target = target;
        }

        #endregion


        #region Override

        public override void Activate()
        {
            base.Activate();

            RemoveAllSubgoals();

            AddSubgoal(new GoalSeek<T>(_owner, _target.transform.position));
            AddSubgoal(new GoalPickup<T>(_owner, _target));
        }

        public override Status Process()
        {
            ActivateIfInactive();

            _status = ProcessSubgoals();

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Terminated GoalGetItem.");
        }

        #endregion
    }
}