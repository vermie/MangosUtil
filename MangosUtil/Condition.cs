using System;

namespace Mangos
{
    public class Condition
    {
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

