using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// プランを把握する記憶オブジェクト
    /// </summary>
    public class Memory
    {
        public PlanBase Plan { get; set; }
        public Vector3 Position { get; set; }
        public GameObject Target { get; set; } 


        #region Constructor

        public Memory()
        {
            //
        }

        public Memory(PlanObject planObject)
        {
            Plan = planObject.Plan;
            Target = planObject.Target;
            Position = planObject.Position;
        }

        #endregion
    }
}
