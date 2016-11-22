using UnityEngine;
using System.Collections;

namespace AI
{
    /// <summary>
    /// 報酬タイプ。
    /// 得られる報酬は、行動エネルギーと攻撃力の2タイプ。
    /// </summary>
    public enum RewardType
    {
        // エネルギー
        Enegy,

        // 攻撃力
        Power,
    }

    /// <summary>
    /// 報酬見込みデータ
    /// </summary>
    public class Reward
    {
        public RewardType RewardType { get; private set; }
        public float Value { get; private set; }

        public Reward(RewardType type, float value)
        {
            RewardType = type;
            Value = value;
        }
    }
}
