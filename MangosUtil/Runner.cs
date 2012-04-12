using System;
using Mangos.Framework;
using Mangos.Scripts;

namespace Mangos
{
    class Runner
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(new ExampleScript().ToSqlInsert());
        }
    }
}
