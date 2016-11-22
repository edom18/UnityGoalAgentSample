using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{

    /// <summary>
    /// 目的地へ移動するゴール
    /// </summary>
    public class GoalSeek<T> : Goal<T> where T : AIBase
    {
        private NavCharacter _character;
        private Vector3 _target;

        #region Constructor

        // コンストラクタ
        public GoalSeek(T owner, Vector3 target) : base(owner)
        {
            _target = target;
        }

        #endregion


        #region Override

        public override void Activate()
        {
            base.Activate();

            _character = _owner.GetComponent<NavCharacter>();
            _character.SetTarget(_target);
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (_character.InNearPoint)
            {
                Debug.Log("In near point. Seek will be completed.");
                _status = Status.Completed;
            }

            return _status;
        }

        public override void Terminate()
        {
            Debug.Log("Terminated GoalSeek.");
        }

        #endregion
    }
}