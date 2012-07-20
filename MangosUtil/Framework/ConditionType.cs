using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mangos.Framework
{
    public enum ConditionType : short
    {
        Not = -3,
        Or = -2,
        And = -1,
        None = 0,
        Aura = 1,
        Item = 2,
        ItemEquipped = 3,
        AreaId = 4,
        ReputationRankMin = 5,
        Team = 6,
        Skill = 7,
        QuestRewarded = 8,
        QuestTaken = 9,
        AdCommissionAura = 10,
        NoAura = 11,
        ActiveGameEvent = 12,
        AreaFlag = 13,
        RaceClass = 14,
        Level = 15,
        NoItem = 16,
        Spell = 17,
        InstanceScript = 18,
        QuestAvailable = 19,
        Achievement = 20,
        AchievementRealm = 21,
        QuestNone = 22,
        ItemWithBank = 23,
        NoItemWithBank = 24,
        NotActiveGameEvent = 25,
        ActiveHoliday = 26,
        NotActiveHoliday = 27,
        LearnableAbility = 28,
        SkillBelow = 29,
        ReputationRankMax = 30,
    }
}
