using System;

namespace Mangos
{
    class Runner
    {
        public static void Main(string[] args)
        {
            Parser.Parse(c => c.IsHorde() && !c.HasAchievement(29) || c.HasAura(123,2));
        }
    }
}
