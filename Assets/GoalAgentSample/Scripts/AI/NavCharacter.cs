using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// NavMeshAgentとThirdPersonCharacterを組み合わせて移動させる
    /// </summary>
    public class NavCharacter : MonoBehaviour
    {
        private AIBase _owner;
        private NavMeshAgent _agent;
        private Vector3 _target;

        [SerializeField]
        private float _baseSpeed = 3.0f;

        public bool IsMoving { get; private set; }

        /// <summary>
        /// 指定されたポイントへ十分近づいたかどうか
        /// </summary>
        public bool InNearPoint
        {
            get
            {
                return _agent.remainingDistance <= _agent.stoppingDistance;
            }
        }


        #region MonoBehaviour

        void Start ()
        {
            _agent = GetComponent<NavMeshAgent>();
            _owner = GetComponent<AIBase>();
        }
        
        void Update ()
        {
            if (_target == null)
            {
                return;
            }

            _agent.speed = _baseSpeed * _owner.Energy + 0.5f;

            if (_agent.destination != _target)
            {
                _agent.SetDestination(_target);
                _agent.Resume();
            }

            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }

        #endregion


        /// <summary>
        /// NavMeshAgentのパスルーティングを再開させる
        /// </summary>
        public void Resume()
        {
            _agent.Resume();
        }

        /// <summary>
        /// NavMeshAgentのパスルーティングを停止させる
        /// </summary>
        public void Stop()
        {
            _agent.Stop();
        }

        /// <summary>
        /// NavMeshAgentのターゲットを設定する
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Vector3 target)
        {
            _target = target;
        }
    }
}
