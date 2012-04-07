using System;

namespace Mangos
{
    class Runner
    {
        public static void Main(string[] args)
        {
            var expr = Parser.Parse(c => (c.HasAura(1234, 1) && c.IsHorde()) || (c.HasAura(1235, 1) && c.IsAlliance()));
        }
    }
}
