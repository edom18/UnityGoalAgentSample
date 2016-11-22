using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// AIが記憶に留められるオブジェクト
    /// </summary>
    public class PlanObject : MonoBehaviour
    {
        public PlanBase Plan { get; private set; }
        public Vector3 Position { get { return transform.position; } }
        public GameObject Target { get { return gameObject; } }

        [SerializeField]
        private GoalType _goalType;

        void Awake()
        {
            switch (_goalType)
            {
                case GoalType.Wander:
                    Plan = new PlanWander();
                    break;

                case GoalType.Seek:
                    Plan = new PlanWander();
                    break;

                case GoalType.GetEnergy:
                    Plan = new PlanGetEnergy();
                    break;

                case GoalType.GetPower:
                    Plan = new PlanGetPower();
                    break;

                case GoalType.Attack:
                    Plan = new PlanAttackTarget();
                    break;

                default:
                    break;
            }
        }
    }
}
