using System;

namespace Mangos
{
    public interface ICondition
    {
        int Id { get; set; }

        int Cond { get; }

        int Val1 { get; }

        int Val2 { get; }
    }

    public class Condition : ICondition
    {
        public int Id
        {
            get;
            set;
        }

        public int Cond
        {
            get;
            set;
        }

        public int Val1
        {
            get;
            set;
        }

        public int Val2
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("[Condition: Cond={0}, Val1={1}, Val2={2}]", Cond, Val1, Val2);
        }
    }
}

