using System;
using Mangos.Framework;

namespace Mangos
{
    class Runner
    {
        public static void Main(string[] args)
        {
            ushort id = 0;
            var expr = Parser.Parse(ref id, c => (c.HasAura(1234, 1) && c.IsHorde()) || (c.HasAura(1235, 1) && c.IsAlliance()));

            foreach (var condtion in expr.Conditions)
                Console.WriteLine(condtion);
        }
    }
}
