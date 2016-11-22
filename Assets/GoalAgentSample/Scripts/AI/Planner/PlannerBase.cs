using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// プランナーインターフェース
    /// </summary>
    interface IPlanner
    {
        float EvaluatePlan(PlanBase plan);
        PlanBase Evaluate(List<PlanBase> plans);
    }

    /// <summary>
    /// プランナーのベースクラス
    ///
    /// プランリストから適切なプランを選択する
    /// </summary>
    public class PlannerBase<T> : IPlanner where T : AIBase
    {
        protected T _owner;

        #region Constructor

        // コンストラクタ
        public PlannerBase(T owner)
        {
            _owner = owner;
        }

        #endregion


        /// <summary>
        /// プランリストを評価して、報酬見込みが一番高いものを返す
        /// </summary>
        /// <param name="plans">評価対象のプランリスト</param>
        /// <returns>選択されたプラン</returns>
        public virtual PlanBase Evaluate(List<PlanBase> plans)
        {
            float maxValue = 0f;
            PlanBase selectedPlan = null;
            foreach (var plan in plans)
            {
                float value = EvaluatePlan(plan);
                if (maxValue <= value)
                {
                    maxValue = value;
                    selectedPlan = plan;
                }
            }

            return selectedPlan;
        }

        /// <summary>
        /// プランを評価する
        /// </summary>
        /// <param name="plan">評価対象のプラン</param>
        /// <returns>オーナーの現在の状態を加味したプランに応じた報酬見込み値</returns>
        public virtual float EvaluatePlan(PlanBase plan)
        {
            return 0f;
        }
    }
}
