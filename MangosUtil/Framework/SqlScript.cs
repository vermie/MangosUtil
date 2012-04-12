using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mangos.Framework
{
    public abstract class SqlScript
    {
        public abstract string FileName { get; }
        public abstract ushort StartingId { get; }

        private ushort _currentId = 0;

        private List<ICondition> _conditions = new List<ICondition>();
        private Dictionary<ushort,string> _comments = new Dictionary<ushort, string>();

        public void AddCondition(string comment, Expression<Func<ConditionFacade,bool>> expression)
        {
            if (_currentId == 0)
            {
                if (StartingId < 100)
                    throw new Exception("StartingId must exceed 99");
                _currentId = StartingId;
            }

            var condition = Parser.Parse(ref _currentId, expression);
            _conditions.Add(condition);

            _comments[condition.Id] = comment;
        }

        public void AddCondition(Expression<Func<ConditionFacade,bool>> expression)
        {
            AddCondition(null, expression);
        }

        public string ToSqlInsert()
        {
            var writer = new StringWriter();
            ToSqlInsert(writer);

            return writer.ToString();
        }

        public void ToSqlInsert(TextWriter writer)
        {
            foreach (var id in _comments.Keys)
                writer.Write("-- {0} => {1}{2}", id, _comments[id], Environment.NewLine);

            writer.WriteLine();
            writer.Write("insert into conditions values");

            var last = _conditions.Last();

            var conditions = from c1 in _conditions
                             from c2 in c1.Conditions
                             select c2;

            foreach (var condition in conditions)
            {
                writer.Write(
                    "{0}  {1}{2}",
                    Environment.NewLine,
                    condition.ToString(),
                    condition == last ? ";" : ",");
            }

            writer.WriteLine();
            writer.Flush();
        }

        public void WriteTo(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new ArgumentException(string.Format(@"""{0}"" does not exist or is not a directory", folderPath));

            using (var writer = new StreamWriter(File.OpenWrite(Path.Combine(folderPath, this.FileName))))
                this.ToSqlInsert(writer);
        }
    }
}

