using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// ゴールの種別
    /// </summary>
    public enum GoalType
    {
        // エネルギーを探すためにうろつく
        Wander,

        // 目的地まで移動する
        Seek,

        // エネルギーを取得する
        GetEnergy,

        // パワーを取得する
        GetPower,

        // 攻撃
        Attack,
    }

    /// <summary>
    /// 行動プランのベースクラス
    /// </summary>
    public class PlanBase
    {
        public GoalType GoalType { get; protected set; }

        /// <summary>
        /// 報酬リスト
        /// </summary>
        public List<Reward> RewardProspects { get; protected set; }


        #region コンストラクタ

        public PlanBase() { }

        public PlanBase(GoalType goalType) : this(goalType, new List<Reward>()) { }

        public PlanBase(GoalType goalType, List<Reward> rewards)
        {
            GoalType = goalType;
            RewardProspects = rewards;
        }

        #endregion
    }

    /// <summary>
    /// あたりをうろつくプラン
    /// </summary>
    public class PlanWander : PlanBase
    {
        public PlanWander() : base(GoalType.Wander) { }
    }

    /// <summary>
    /// パワーを得る
    /// </summary>
    public class PlanGetPower : PlanBase
    {
        public PlanGetPower() : base(GoalType.GetPower)
        {
            var reward = new Reward(RewardType.Power, 0.1f);
            RewardProspects.Add(reward);
        }
    }

    /// <summary>
    /// エネルギーを得る
    /// </summary>
    public class PlanGetEnergy : PlanBase
    {
        public PlanGetEnergy() : base(GoalType.GetEnergy)
        {
            var reward = new Reward(RewardType.Enegy, 0.1f);
            RewardProspects.Add(reward);
        }
    }

    public class PlanAttackTarget : PlanBase
    {
        public PlanAttackTarget() : base(GoalType.Attack, new List<Reward>()) { }
    }
}
