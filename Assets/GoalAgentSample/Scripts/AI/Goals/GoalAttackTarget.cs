using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// ターゲットを攻撃するゴール
    /// </summary>
    public class GoalAttackTarget<T> : CompositeGoal<T> where T : AIBase
    {
        private float _canAttackDistance = 2.0f;
        private GameObject _target;
        

        #region コンストラクタ

        public GoalAttackTarget(T owner, GameObject target) : base(owner)
        {
            _target = target;
        }

        #endregion


        #region Override Goal Methods

        public override void Activate()
        {
            base.Activate();

            RemoveAllSubgoals();

            var pos = _target.transform.position;
            pos.y = _owner.transform.position.y;
            var dir = (_owner.transform.position - pos).normalized;
            pos += dir * _canAttackDistance;
            AddSubgoal(new GoalSeek<T>(_owner, pos));
            AddSubgoal(new GoalAttack<T>(_owner, _target, pos));
        }

        public override Status Process()
        {
            ActivateIfInactive();

            _status = ProcessSubgoals();

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Has been terminated of GoalAttackTarget.");
        }

        #endregion
    }
}
