using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mangos.Framework
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
        public bool HasAura(uint spellId, uint effectIndex)
        {
            return true;
        }

        private static Condition ParseHasAura(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = (not ? ConditionType.NoAura : ConditionType.Aura),
                Val1 = (uint)args[0].Value,
                Val2 = (uint)args[1].Value
            };
        }

        #endregion

        #region CONDITION_ITEM, CONDITION_NO_ITEM, CONDITION_ITEM_WITH_BANK, CONDITION_NOITEM_WITH_BANK

        /// <summary>
        /// Use <example>!HasItem(...)</example> to validate the non-existence of specified item.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasItem(uint itemId, uint count, bool searchBank)
        {
            return true;
        }

        private static Condition ParseHasItem(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            bool searchBank = (bool)args[2].Value;

            ConditionType cond;

            if (searchBank)
                cond = not ? ConditionType.NoItemWithBank : ConditionType.ItemWithBank;
            else
                cond = not ? ConditionType.NoItem : ConditionType.Item;

            return new Condition()
            {
                Cond = cond,
                Val1 = (uint)args[0].Value,
                Val2 = (uint)args[1].Value
            };
        }

        #endregion

        #region CONDITION_ITEM_EQUIPPED

        [Condition]
        public bool HasItemEquipped(uint itemId)
        {
            return true;
        }

        private static Condition ParseHasItemEquipped(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.ItemEquipped,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_AREAID

        [Condition]
        public bool IsInArea(uint areaId)
        {
            return true;
        }

        private static Condition ParseIsInArea(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.AreaId,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_REPUTATION_RANK

        [Condition]
        public bool HasReputationRank(uint factionId, uint minRank)
        {
            return true;
        }

        private static Condition ParseHasReputationRank(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = ConditionType.ReputationRankMin,
                Val1 = (uint)args[0].Value,
                Val2 = (uint)args[1].Value
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

        private static Condition ParseTeam(uint team)
        {
            return new Condition()
            {
                Cond = ConditionType.Team,
                Val1 = team,
                Val2 = 0
            };
        }

        private static Condition ParseIsHorde(MethodCallExpression expr, bool not)
        {
            return ParseTeam(67u);
        }

        private static Condition ParseIsAlliance(MethodCallExpression expr, bool not)
        {
            return ParseTeam(469u);
        }

        #endregion

        #region CONDITION_SKILL

        [Condition]
        public bool IsSkillAtLevel(uint skillId, uint minLevel)
        {
            return true;
        }

        private static Condition ParseIsSkillAtLevel(MethodCallExpression expr, bool not)
        {
            var args = expr.Arguments.Cast<ConstantExpression>().ToArray();

            return new Condition()
            {
                Cond = ConditionType.Skill,
                Val1 = (uint)args[0].Value,
                Val2 = (uint)args[1].Value
            };
        }

        #endregion

        #region CONDITION_QUESTREWARDED

        [Condition]
        public bool HasCompletedQuest(uint questId)
        {
            return true;
        }

        private static Condition ParseHasCompletedQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.QuestRewarded,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_QUESTTAKEN

        [Condition]
        public bool IsOnQuest(uint questId)
        {
            return true;
        }

        private static Condition ParseIsOnQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.QuestTaken,
                Val1 = (uint)arg.Value,
                Val2 = 0u
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
                Cond = ConditionType.AdCommissionAura,
                Val1 = 0u,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_ACTIVE_GAME_EVENT, CONDITION_NOT_ACTIVE_GAME_EVENT

        /// <summary>
        /// Use <example>!IsEventActive(...)</example> to validate the event is inactive.
        /// </summary>
        [Condition(Negatable = true)]
        public bool IsEventActive(uint eventId)
        {
            return true;
        }

        private static Condition ParseIsEventActive(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = not ? ConditionType.NotActiveGameEvent : ConditionType.ActiveGameEvent,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_AREA_FLAG

        /// <summary>
        /// Use <example>!HasAreaFlag(...)</example> to validate the non-existence of flags.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasAreaFlag(uint areaFlags)
        {
            return true;
        }

        private static Condition ParseHasAreaFlag(MethodCallExpression method, bool not)
        {
            var arg = (ConstantExpression)method.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.AreaFlag,
                Val1 = not ? 0xFFu : (uint)arg.Value,
                Val2 = not ? (uint)arg.Value : 0xFFu
            };
        }

        #endregion

        #region CONDITION_RACE_CLASS

        [Condition]
        public bool HasRace(uint raceFlags)
        {
            return true;
        }

        [Condition]
        public bool HasClass(uint classFlags)
        {
            return true;
        }

        private static Condition ParseHasRace(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.RaceClass,
                Val1 = (uint)arg.Value,
                Val2 = 0xFFu
            };
        }

        private static Condition ParseHasClass(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.RaceClass,
                Val1 = 0xFFu,
                Val2 = (uint)arg.Value
            };
        }

        #endregion

        #region CONDITION_LEVEL

        [Condition]
        public bool IsLevel(uint level)
        {
            return true;
        }

        [Condition]
        public bool IsAtLeastLevel(uint level)
        {
            return true;
        }

        [Condition]
        public bool IsOverLevel(uint level)
        {
            return true;
        }

        private static Condition ParseLevel(MethodCallExpression method, uint val2)
        {
            var arg = (ConstantExpression)method.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.Level,
                Val1 = (uint)arg.Value,
                Val2 = val2
            };
        }

        private static Condition ParseIsLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 0u);
        }

        private static Condition ParseIsAtLeastLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 1u);
        }

        private static Condition ParseIsOverLevel(MethodCallExpression method, bool not)
        {
            return ParseLevel(method, 2u);
        }

        #endregion

        #region CONDITION_SPELL

        /// <summary>
        /// Use <example>!KnowsSpell(...)</example> to validate the non-existence of specified spell.
        /// </summary>
        [Condition(Negatable = true)]
        public bool KnowsSpell(uint spellId)
        {
            return true;
        }

        private static Condition ParseKnowsSpell(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.Spell,
                Val1 = (uint)arg.Value,
                Val2 = not ? 1u : 0u
            };
        }

        #endregion

        #region CONDITION_QUESTAVAILABLE

        [Condition]
        public bool CanAcceptQuest(uint questId)
        {
            return true;
        }

        private static Condition ParseCanAcceptQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.QuestAvailable,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_ACHIEVEMENT

        /// <summary>
        /// Use <example>!HasAchievement(...)</example> to validate the non-existence of specified achievement.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasAchievement(uint achievementId)
        {
            return true;
        }

        private static Condition ParseHasAchievement(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.Achievement,
                Val1 = (uint)arg.Value,
                Val2 = not ? 1u : 0u
            };
        }

        #endregion

        #region CONDITION_ACHIEVEMENT_REALM

        /// <summary>
        /// Use <example>!HasAchievement(...)</example> to validate the non-existence of specified achievement.
        /// </summary>
        [Condition(Negatable = true)]
        public bool HasRealmAchievement(uint achievementId)
        {
            return true;
        }

        private static Condition ParseHasRealmAchievement(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.AchievementRealm,
                Val1 = (uint)arg.Value,
                Val2 = not ? 1u : 0u
            };
        }

        #endregion

        #region CONDITION_QUESTNONE

        [Condition]
        public bool HasNeverHadQuest(uint questId)
        {
            return true;
        }

        private static Condition ParseHasNeverHadQuest(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = ConditionType.QuestNone,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

        #region CONDITION_ACTIVE_HOLIDAY, CONDITION_NOT_ACTIVE_HOLIDAY

        /// <summary>
        /// Use <example>!IsHolidayActive(...)</example> to validate the holiday is inactive.
        /// </summary>
        [Condition(Negatable = true)]
        public static bool IsHolidayActive(uint eventId)
        {
            return true;
        }

        private static Condition ParseIsHolidayActive(MethodCallExpression expr, bool not)
        {
            var arg = (ConstantExpression)expr.Arguments[0];

            return new Condition()
            {
                Cond = not ? ConditionType.NotActiveHoliday : ConditionType.ActiveHoliday,
                Val1 = (uint)arg.Value,
                Val2 = 0u
            };
        }

        #endregion

    }
}

