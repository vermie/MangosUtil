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

        public sealed override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Id, Cond, Val1, Val2);
        }
    }

    public abstract class BinaryCondition : ICondition
    {
        public int Id
        {
            get;
            set;
        }

        public abstract int Cond { get; }

        public int Val1 { get { return _lhs.Id; } }

        public int Val2 { get { return _rhs.Id; } }

        private ICondition _lhs;
        private ICondition _rhs;

        public BinaryCondition(ICondition lhs, ICondition rhs)
        {
            _lhs = lhs;
            _rhs = rhs;
        }

        public sealed override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Id, Cond, Val1, Val2);
        }
    }

    public class OrCondition : BinaryCondition
    {
        public override int Cond { get { return -1; } }

        public OrCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }

    public class AndCondition : BinaryCondition
    {
        public override int Cond { get { return -2; } }

        public AndCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }
}

