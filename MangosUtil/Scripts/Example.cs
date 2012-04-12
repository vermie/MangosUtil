using System;
using System.Linq.Expressions;
using Mangos.Framework;

namespace Mangos.Scripts
{
    public class ExampleScript : SqlScript
    {
        public override string FileName { get { return "example.sql"; } }

        public override ushort StartingId { get { return 100; } }

        public ExampleScript()
        {
            this.AddCondition("big expression", c => (c.HasAura(1234, 1) && c.IsHorde()) || (c.HasAura(1235, 1) && c.IsAlliance()));
            
            this.AddCondition("large condition", c => (!c.HasAchievement(5678) && c.HasAura(1234, 0)) ||
                                                      (c.HasAreaFlag(1234) || c.HasItem(1234, 20) || c.IsAlliance()));

            this.AddCondition("complicated thing", c => (c.IsHorde() && c.IsOverLevel(70)) ||
                                                        ((!c.HasAchievement(5678) && c.HasAura(1234, 0)) ||
                                                         (c.HasAreaFlag(1234) || c.HasItem(1234, 20) || c.IsAlliance())));
        }
    }
}

