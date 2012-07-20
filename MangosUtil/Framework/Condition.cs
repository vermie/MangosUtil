using System;
using System.Collections.Generic;
using System.Text;

namespace Mangos.Framework
{
    public interface ICondition
    {
        ushort Id { get; set; }

        ConditionType Cond { get; }

        uint Val1 { get; }

        uint Val2 { get; }

        IEnumerable<ICondition> Conditions { get; }
    }

    public static class ConditionExtensions
    {
        internal static string ToStringImpl(this ICondition condition)
        {
            return string.Format("({0}, {1}, {2}, {3})", condition.Id, condition.Cond, condition.Val1, condition.Val2);
        }

        public static string ToSqlInsert(this ICondition condition)
        {
            var sb = new StringBuilder();

            sb.Append("insert into conditions values");

            foreach (var c in condition.Conditions)
            {
                sb.AppendFormat(
                    "{0}  {1}{2}",
                    Environment.NewLine,
                    c.ToString(),
                    c == condition ? ";" : ",");
            }

            return sb.AppendLine().ToString();
        }
    }

    public class Condition : ICondition
    {
        public ushort Id
        {
            get;
            set;
        }

        public ConditionType Cond
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
            return this.ToStringImpl();
        }
    }

    public class NotCondition : ICondition
    {
        public ushort Id { get; set; }

        public ConditionType Cond { get { return ConditionType.Not; } }

        public uint Val1 { get { return _cond.Id; } }

        public uint Val2 { get { return 0; } }

        public IEnumerable<ICondition> Conditions
        {
            get
            {
                foreach (var condition in _cond.Conditions)
                    yield return condition;

                yield return this;
            }
        }

        private ICondition _cond;

        public NotCondition(ICondition cond)
        {
            _cond = cond;
        }

        public sealed override string ToString()
        {
            return this.ToStringImpl();
        }
    }

    public abstract class BinaryCondition : ICondition
    {
        public ushort Id
        {
            get;
            set;
        }

        public abstract ConditionType Cond { get; }

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
            return this.ToStringImpl();
        }
    }

    public class OrCondition : BinaryCondition
    {
        public override ConditionType Cond { get { return ConditionType.Or; } }

        public OrCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }

    public class AndCondition : BinaryCondition
    {
        public override ConditionType Cond { get { return ConditionType.And; } }

        public AndCondition(ICondition lhs, ICondition rhs) : base(lhs, rhs)
        {
        }
    }
}

