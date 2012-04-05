using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mangos
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConditionAttribute : Attribute
    {
        public bool Negatable
        {
            get;
            set;
        }
    }
    
    public class ConditionFacade
    {
        #region CONDITION_AURA, CONDITION_NO_AURA

        /// <summary>
        /// Use <example>!HasAura(...)</example> to validate the non-existence of specified aura.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasAura(int spellId, int effectIndex)
        {
            return true;
        }

        private static Condition ParseHasAura(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = not ? 11 : 1,
                Val1 = (int)args[0].Value,
                Val2 = (int)args[1].Value
            };
        }

        #endregion

        #region CONDITION_ITEM, CONDITION_NO_ITEM

        /// <summary>
        /// Use <example>!HasItem(...)</example> to validate the non-existence of specified item.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasItem(int itemId, int count)
        {
            return true;
        }

        private static Condition ParseHasItem(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = not ? 16 : 2,
                Val1 = (int)args[0].Value,
                Val2 = (int)args[1].Value
            };
        }

        #endregion

        #region CONDITION_ITEM_EQUIPPED

        [Condition]
        public bool HasItemEquipped(int itemId)
        {
            return true;
        }

        private static Condition ParseHasItemEquipped(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 3,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_AREAID

        [Condition]
        public bool IsInArea(int areaId)
        {
            return true;
        }

        private static Condition ParseIsInArea(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 4,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_REPUTATION_RANK

        [Condition]
        public bool HasReputationRank(int factionId, int minRank)
        {
            return true;
        }

        private static Condition ParseHasReputationRank(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = 5,
                Val1 = (int)args[0].Value,
                Val2 = (int)args[1].Value
            };
        }

        #endregion

        #region CONDITION_TEAM

        [Condition]
        public bool IsHorde()
        {
            return true;
        }

        [Condition]
        public bool IsAlliance()
        {
            return true;
        }

        private static Condition ParseTeam(int team)
        {
            return new Condition()
            {
                Cond = 6,
                Val1 = team,
                Val2 = 0
            };
        }

        private static Condition ParseIsHorde(MethodCallExpression expr, bool not)
        {
            return ParseTeam(67);
        }

        private static Condition ParseIsAlliance(MethodCallExpression expr, bool not)
        {
            return ParseTeam(469);
        }

        #endregion

        #region CONDITION_SKILL

        [Condition]
        public bool IsSkillAtLevel(int skillId, int minLevel)
        {
            return true;
        }

        private static Condition ParseIsSkillAtLevel(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = 7,
                Val1 = (int)args[0].Value,
                Val2 = (int)args[1].Value
            };
        }

        #endregion

        #region CONDITION_QUESTREWARDED

        [Condition]
        public bool HasCompletedQuest(int questId)
        {
            return true;
        }

        private static Condition ParseHasCompletedQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 8,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_QUESTTAKEN

        [Condition]
        public bool IsOnQuest(int questId)
        {
            return true;
        }

        private static Condition ParseIsOnQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 9,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_AD_COMMISSION_AURA

        [Condition]
        public bool HasAdCommissionAura()
        {
            return true;
        }

        private static Condition ParseHasAdCommissionAura(MethodCallExpression expr, bool not)
        {
            return new Condition()
            {
                Cond = 10,
                Val1 = 0,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_ACTIVE_EVENT

        [Condition]
        public bool IsEventActive(int eventId)
        {
            return true;
        }

        private static Condition ParseIsEventActive(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 12,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_AREA_FLAG

        /// <summary>
        /// Use <example>!HasAreaFlag(...)</example> to validate the non-existence of flags.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasAreaFlag(int areaFlags)
        {
            return true;
        }

        private static Condition ParseHasAreaFlag(MethodCallExpression method, bool not)
        {
            var arg = (ConstantExpression)method.Arguments[0];

            return new Condition()
            {
                Cond = 13,
                Val1 = not ? 0xFF : (int)arg.Value,
                Val2 = not ? (int)arg.Value : 0xFF
            };
        }

        #endregion

        #region CONDITION_RACE_CLASS

        [Condition]
        public bool HasRace(int raceFlags)
        {
            return true;
        }

        [Condition]
        public bool HasClass(int classFlags)
        {
            return true;
        }

        private static Condition ParseHasRace(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 14,
                Val1 = (int)arg.Value,
                Val2 = 0xFF
            };
        }

        private static Condition ParseHasClass(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 14,
                Val1 = 0xFF,
                Val2 = (int)arg.Value
            };
        }

        #endregion

        #region CONDITION_LEVEL

        [Condition]
        public bool IsLevel(int level)
        {
            return true;
        }

        [Condition]
        public bool IsAtLeastLevel(int level)
        {
            return true;
        }

        [Condition]
        public bool IsOverLevel(int level)
        {
            return true;
        }

        private static Condition ParseLevel(MethodCallExpression method, int val2)
        {
            var arg = (ConstantExpression)method.Arguments[0];

            return new Condition()
            {
                Cond = 15,
                Val1 = (int)arg.Value,
                Val2 = val2
            };
        }

        private static Condition ParseIsLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 0);
        }

        private static Condition ParseIsAtLeastLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 1);
        }

        private static Condition ParseIsOverLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 2);
        }

        #endregion

        #region CONDITION_SPELL

        /// <summary>
        /// Use <example>!KnowsSpell(...)</example> to validate the non-existence of specified spell.
        /// </summary>
        [Condition(Negatable = true)]
        public bool KnowsSpell(int spellId)
        {
            return true;
        }

        private static Condition ParseKnowsSpell(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 17,
                Val1 = (int)arg.Value,
                Val2 = not ? 1 : 0
            };
        }

        #endregion

        #region CONDITION_QUESTAVAILABLE

        [Condition]
        public bool CanAcceptQuest(int questId)
        {
            return true;
        }

        private static Condition ParseCanAcceptQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 19,
                Val1 = (int)arg.Value,
                Val2 = 0
            };
        }

        #endregion

        #region CONDITION_ACHIEVEMENT

        /// <summary>
        /// Use <example>!HasAchievement(...)</example> to validate the non-existence of specified achievement.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasAchievement(int achievementId)
        {
            return true;
        }

        private static Condition ParseHasAchievement(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 20,
                Val1 = (int)arg.Value,
                Val2 = not ? 1 : 0
            };
        }

        #endregion

        #region CONDITION_ACHIEVEMENT_REALM

        /// <summary>
        /// Use <example>!HasAchievement(...)</example> to validate the non-existence of specified achievement.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasRealmAchievement(int achievementId)
        {
            return true;
        }

        private static Condition ParseHasRealmAchievement(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = 21,
                Val1 = (int)arg.Value,
                Val2 = not ? 1 : 0
            };
        }

        #endregion
    }
}

