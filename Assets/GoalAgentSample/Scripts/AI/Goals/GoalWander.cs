using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// あたりを適当にうろつくゴール
    ///
    /// 各ポイントへの移動は`GoalSeek`で行う
    /// </summary>
    public class GoalWander<T> : CompositeGoal<T> where T : AIBase
    {
        //private NavCharacter _character;
        private Transform[] _targets;

        /// <summary>
        /// 探索ポイントが空か
        /// </summary>
        bool IsEmpty { get { return _targets.Length == 0; } }


        #region Constructor

        // コンストラクタ
        public GoalWander(T owner) : base(owner) { }

        #endregion


        #region Override

        public override void Activate()
        {
            Debug.Log("Activate GoalWander.");

            base.Activate();

            RemoveAllSubgoals();

            _targets = _owner.WanderTargets;

            if (IsEmpty)
            {
                _status = Status.Failed;
                return;
            }

            // 次の探索位置
            Vector3 next = PickupNextTarget().position;

            AddSubgoal(new GoalSeek<T>(_owner, next));
        }

        public override Status Process()
        {
            ActivateIfInactive();

            // ターゲットがない場合は失敗
            if (IsEmpty)
            {
                return Status.Failed;
            }

            _status = ProcessSubgoals();

            ReactivateIfFailed();

            ReactivateIfCompleted();

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Terminated GoalWander.");
        }

        #endregion


        /// <summary>
        /// 完了した場合は非アクティブにする（再検索を開始する）
        /// （基本的に探索は無限ループで常に新しいポイントを探し続ける）
        /// </summary>
        void ReactivateIfCompleted()
        {
            if (IsCompleted)
            {
                _status = Status.Inactive;
            }
        }

        /// <summary>
        /// 次の探索地点を返す（ランダム）
        /// </summary>
        /// <returns></returns>
        Transform PickupNextTarget()
        {
            if (IsEmpty)
            {
                return null;
            }

            int index = Random.Range(0, _targets.Length);
            return _targets[index];
        }
    }
}
