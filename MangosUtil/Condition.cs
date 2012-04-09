using System;
using System.Collections.Generic;

namespace Mangos
{
    public interface ICondition
    {
        ushort Id { get; set; }

        short Cond { get; }

        uint Val1 { get; }

        uint Val2 { get; }

        IEnumerable<ICondition> Conditions { get; }
    }

    public class Condition : ICondition
    {
        public ushort Id
        {
            get;
            set;
        }

        public short Cond
        {
            get;
            set;
        }

        public uint Val1
        {
            get;
            set;
        }

        public uint Val2
        {
            get;
            set;
        }

        public IEnumerable<ICondition> Conditions { get { yield return this; } }

        public sealed override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Id, Cond, Val1, Val2);
        }
    }

    public abstract class BinaryCondition : ICondition
    {
        public ushort Id
        {
            get;
            set;
        }

        public abstract short Cond { get; }

        public uint Val1 { get { return _lhs.Id; } }

        public uint Val2 { get { return _rhs.Id; } }

        public IEnumerable<ICondition> Conditions
        {
            get
            {
                foreach (var condition in _lhs.Conditions)
                    yield return condition;

                foreach (var condition in _rhs.Conditions)
                    yield return condition;

                yield return this;
            }
        }

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
        public override short Cond { get { return -1; } }

        public OrCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }

    public class AndCondition : BinaryCondition
    {
        public override short Cond { get { return -2; } }

        public AndCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }
}

