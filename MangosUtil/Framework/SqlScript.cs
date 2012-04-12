using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mangos.Framework
{
    public abstract class SqlScript
    {
        public abstract string FileName { get; }
        public abstract ushort StartingId { get; }
        public ushort CurrentId { get; private set; }

        private List<ICondition> _conditions = new List<ICondition>();
        private Dictionary<ushort,string> _comments = new Dictionary<ushort, string>();

        public void AddCondition(string comment, Expression<Func<ConditionFacade,bool>> expression)
        {
            var currentId = CurrentId;
            var condition = Parser.Parse(ref currentId, expression);
            _conditions.Add(condition);
            CurrentId = currentId;

            _comments[condition.Id] = comment;
        }

        public void AddCondition(Expression<Func<ConditionFacade,bool>> expression)
        {
            AddCondition(null, expression);
        }

        public string ToSqlInsert()
        {
            var sb = new StringBuilder();

            foreach (var id in _comments.Keys)
                sb.AppendFormat("-- {0} => {1}{2}", id, _comments[id], Environment.NewLine);

            sb.AppendLine();
            sb.Append("insert into conditions values");

            var last = _conditions.Last();

            var conditions = from c1 in _conditions
                             from c2 in c1.Conditions
                             select c2;

            foreach (var condition in conditions)
            {
                sb.AppendFormat(
                    "{0}  {1}{2}",
                    Environment.NewLine,
                    condition.ToString(),
                    condition == last ? ";" : ",");
            }

            return sb.AppendLine().ToString();
        }
    }
}

