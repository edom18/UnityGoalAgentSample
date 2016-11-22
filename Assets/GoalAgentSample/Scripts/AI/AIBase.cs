using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// AIの基底クラス
    ///
    /// 各種パラメータなど、UI判断に必要な情報を集約する。
    /// 基本的に、Brainクラスがゴールを実行し、
    /// PlannerBaseクラスがゴールをプランニングすることでAIを実現する。
    /// </summary>
    public class AIBase : MonoBehaviour
    {
        private Brain<AIBase> _brain;

        [SerializeField]
        private Transform[] _wanderTargets;
        public Transform[] WanderTargets { get { return _wanderTargets; } }

        // 現在の攻撃力（攻撃ごとに減る）
        [SerializeField][Range(0f, 1f)]
        private float _attackPower = 0.5f;
        public float AttackPower
        {
            get
            {
                return _attackPower;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _attackPower = value;
            }
        }

        // 活動エネルギー。行動するのに必要
        [SerializeField][Range(0f, 1f)]
        private float _energy = 0.5f;
        public float Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _energy = value;
            }
        }

        // 弾丸
        [SerializeField]
        private GameObject _bulletPrefab;
        public GameObject BulletPrefab { get { return _bulletPrefab; } }

        /// <summary>
        /// 対象がプランオブジェクトを保持しているかを検証
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <param name="planObject">取得したプランオブジェクトの参照を返す</param>
        /// <returns>保持している場合はture</returns>
        protected virtual bool HasPlan(GameObject target, out PlanObject planObject)
        {
            planObject = target.GetComponent<PlanObject>();
            return planObject != null;
        }

        /// <summary>
        /// プランオブジェクトを保持する（記憶する）
        /// </summary>
        /// <param name="planObject"></param>
        void StorePlanObject(PlanObject planObject)
        {
            _brain.Memorize(planObject);
        }
        
        /// <summary>
        /// 移動にはコストを支払う
        /// </summary>
        void Charge()
        {
            Energy -= 0.0001f;
        }

        #region MonoBehaviour

        protected virtual void Start ()
        {
            _brain = new Brain<AIBase>(this);
        }
        
        protected virtual void Update ()
        {
            Charge();

            _brain.Process();
            _brain.MemoryControl();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            PlanObject planObject;
            if (!HasPlan(other.gameObject, out planObject))
            {
                return;
            }

            Debug.Log("OnTriggerEnter with " + planObject);

            StorePlanObject(planObject);
        }

        #endregion
    }
}
