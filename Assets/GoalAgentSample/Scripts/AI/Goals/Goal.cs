using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// ゴールの状態
    /// </summary>
    public enum Status
    {
        Inactive,
        Active,
        Completed,
        Failed,
    }

    /// <summary>
    /// ゴールインターフェース
    /// </summary>
    public interface IGoal
    {
        bool IsInactive { get; }
        bool IsActive { get; }
        bool IsCompleted { get; }
        bool HasFailed { get; }

        void Activate();
        Status Process();
        void Terminate();
        void AddSubgoal(IGoal subgoal);
    }

    /// <summary>
    /// ゴールの基底クラス
    /// </summary>
    public class Goal<T> : IGoal where T : AIBase
    {
        protected T _owner;

        /// <summary>
        /// 非アクティブか
        /// </summary>
        public bool IsInactive { get { return _status == Status.Inactive; } }

        /// <summary>
        /// アクティブか
        /// </summary>
        public bool IsActive { get { return _status == Status.Active; } }

        /// <summary>
        /// 完了済か
        /// </summary>
        public bool IsCompleted { get { return _status == Status.Completed; } }

        /// <summary>
        /// ゴール失敗か
        /// </summary>
        public bool HasFailed { get { return _status == Status.Failed; } }

        /// <summary>
        /// 現在のステータス
        /// </summary>
        internal Status _status = Status.Inactive;

        public Goal(T owner)
        {
            _owner = owner;
        }


        /// <summary>
        /// 非アクティブならアクティブ状態に移行する
        /// </summary>
        internal void ActivateIfInactive()
        {
            if (IsInactive)
            {
                Activate();
            }
        }

        /// <summary>
        /// 失敗している場合はアクティブ化を試みる
        /// </summary>
        protected void ReactivateIfFailed()
        {
            if (HasFailed)
            {
                _status = Status.Inactive;
            }
        }

        /// <summary>
        /// アクティベイト処理
        /// </summary>
        public virtual void Activate()
        {
            Debug.Log("Start " + this);

            _status = Status.Active;
        }

        public virtual Status Process()
        {
            ActivateIfInactive();
            return _status;
        } 

        /// <summary>
        /// ゴールの後処理
        /// 成功／失敗に関わらず実行される
        /// </summary>
        public virtual void Terminate()
        {
            // do nothing.
        }

        public virtual void AddSubgoal(IGoal subgoal)
        {
            // do nothing.
        }
    }

    /// <summary>
    /// サブゴールを持つゴール
    /// </summary>
    public class CompositeGoal<T> : Goal<T> where T : AIBase
    {
        /// <summary>
        /// サブゴールのリスト
        /// </summary>
        protected List<IGoal> _subgoals = new List<IGoal>();

        // コンストラクタ
        public CompositeGoal(T owner) : base(owner) { }

        #region Override
        public override void Activate()
        {
            base.Activate();
        }

        public override Status Process()
        {
            ActivateIfInactive();
            return ProcessSubgoals();
        }

        public override void Terminate()
        {
            base.Terminate();
        }
        #endregion


        /// <summary>
        /// サブゴールを追加
        /// </summary>
        /// <param name="subgoal"></param>
        public override void AddSubgoal(IGoal subgoal)
        {
            if (_subgoals.Contains(subgoal))
            {
                return;
            }

            _subgoals.Add(subgoal);
        }

        /// <summary>
        /// すべてのサブゴールを終了させ、クリアする
        /// </summary>
        protected void RemoveAllSubgoals()
        {
            foreach (var goal in _subgoals)
            {
                goal.Terminate();
            }

            _subgoals.Clear();
        }

        /// <summary>
        /// サブゴールを評価する
        /// </summary>
        /// <returns></returns>
        internal virtual Status ProcessSubgoals()
        {
            // サブゴールリストの中で完了 or 失敗のゴールをすべて終了させ、リストから削除する
            while (_subgoals.Count > 0 &&
                  (_subgoals[0].IsCompleted || _subgoals[0].HasFailed))
            {
                _subgoals[0].Terminate();
                _subgoals.RemoveAt(0);
            }

            // サブゴールがなくなったら完了。
            if (_subgoals.Count == 0)
            {
                _status = Status.Completed;
                return _status;
            }

            var firstGoal = _subgoals[0];

            // 残っているサブゴールの最善のゴールを評価する
            var subgoalStatus = firstGoal.Process();

            // 最前のゴールが完了していて、かつまだサブゴールが残っている場合は処理を継続する
            if ((subgoalStatus == Status.Completed) &&
                _subgoals.Count > 1)
            {
                _status = Status.Active;
                return _status;
            }

            return _status;
        } 
    }
}
