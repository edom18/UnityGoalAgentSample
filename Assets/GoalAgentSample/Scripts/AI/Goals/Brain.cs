using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    /// <summary>
    /// ゴールを統括するルートゴール
    /// </summary>
    public class Brain<T> : CompositeGoal<T> where T : AIBase
    {
        // プランナー
        private IPlanner _planner;

        // 現在選択されているプラン
        private PlanBase _currentPlan;

        // 短期記憶しているオブジェクトを保持
        private List<Memory> _shortMemories = new List<Memory>();

        // 長期記憶しているオブジェクトを保持
        private List<Memory> _longMemories = new List<Memory>();

        /// <summary>
        /// 記憶しているすべてのオブジェクトを返す
        /// </summary>
        private List<Memory> AllMemories
        {
            get
            {
                List<Memory> allMemories = new List<Memory>();
                allMemories.AddRange(_shortMemories);
                allMemories.AddRange(_longMemories);
                return allMemories;
            }
        }

        #region Constructor

        // コンストラクタ
        public Brain(T owner) : base(owner)
        {
            // NOTE:
            // 他キャラのAIを作成する場合はBrainの派生クラスを作成し、
            // そちらのコンストラクタ内で、生成するプランナーを変更する
            _planner = new CharaPlanner(owner);
        }

        #endregion


        #region Public members

        /// <summary>
        /// プランを記憶に保持
        /// </summary>
        /// <param name="planObject"></param>
        public void Memorize(PlanObject planObject)
        {
            // 重複しているプランは追加しない
            if (HasMemory(planObject))
            {
                return;
            }

            _shortMemories.Add(MakeMemory(planObject));
        }

        /// <summary>
        /// メモリコントロール
        /// すでに達成したプランなど、記憶から消すべきオブジェクトをリストから削除する
        /// </summary>
        public void MemoryControl()
        {
            var targets = from m in _shortMemories
                          where m.Target != null
                          select m;

            var newList = targets.ToList();
            _shortMemories = newList;
        }

        #endregion


        #region Private members

        /// <summary>
        /// プランリストから最適なプランを評価、取得する
        /// </summary>
        /// <returns></returns>
        PlanBase EvaluatePlans()
        {
            List<PlanBase> plans = EnumeratePlans();
            return _planner.Evaluate(plans);
        }

        /// <summary>
        /// 短期・長期記憶双方に保持しているプランを列挙する
        /// </summary>
        /// <returns></returns>
        List<PlanBase> EnumeratePlans()
        {
            var plans = new List<PlanBase>();
            foreach (var m in AllMemories)
            {
                plans.Add(m.Plan);
            }
            return plans;
        }

        /// <summary>
        /// プランに応じてゴールを選択する
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        IGoal GetGoalByPlan(PlanBase plan)
        {
            switch (plan.GoalType)
            {
                // あたりを探し回る
                case GoalType.Wander:
                {
                    return new GoalWander<T>(_owner);
                }

                // エネルギー／パワーを得る
                case GoalType.GetEnergy:
                case GoalType.GetPower:
                {
                    var memory = FindMemory(plan);
                    return new GoalGetItem<T>(_owner, memory.Target);
                }

                // 敵を攻撃
                case GoalType.Attack:
                {
                    var memory = FindMemory(plan);
                    return new GoalAttackTarget<T>(_owner, memory.Target);
                }
            }
            
            return new Goal<T>(_owner);
        }

        /// <summary>
        /// 選択中のプランからプランを変更する
        /// </summary>
        /// <param name="newPlan"></param>
        void ChangePlan(PlanBase newPlan)
        {
            Debug.Log("Change plan to " + newPlan);

            _currentPlan = newPlan;
            RemoveAllSubgoals();

            var goal = GetGoalByPlan(newPlan);
            AddSubgoal(goal);
        }

        /// <summary>
        /// プランオブジェクトからメモリオブジェクトを生成する
        /// </summary>
        Memory MakeMemory(PlanObject planObject)
        {
            var memory = new Memory(planObject);
            return memory;
        }

        /// <summary>
        /// 対象プランから記憶オブジェクトを検索
        /// </summary>
        Memory FindMemory(PlanBase plan)
        {
            return AllMemories.Find(m => m.Plan == plan);
        }

        /// <summary>
        /// 記憶にあるプランか
        /// </summary>
        bool HasMemory(PlanObject planObject)
        {
            var memory = AllMemories.Find(m => m.Plan == planObject.Plan);
            return memory != null;
        }

        /// <summary>
        /// 記憶にあるメモリか
        /// </summary>
        bool HasMemory(Memory memory)
        {
            var storeMem = AllMemories.Find(m => m == memory);
            return storeMem != null;
        }

        #endregion


        #region Override Goal class

        public override void Activate()
        {
            base.Activate();

            RemoveAllSubgoals();

            // なにもないときにあたりを歩き回るプランを設定しておく
            var memory = new Memory();
            memory.Plan = new PlanWander();

            _longMemories.Add(memory);
        }

        public override Status Process()
        {
            ActivateIfInactive();

            PlanBase selectedPlan = EvaluatePlans();
            bool needsChangePlan = (selectedPlan != null) && (_currentPlan != selectedPlan);
            if (needsChangePlan)
            {
                ChangePlan(selectedPlan);
            }

            return ProcessSubgoals();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        #endregion
    }
}
