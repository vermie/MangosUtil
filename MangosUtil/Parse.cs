using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ParseDelegate = System.Func<System.Linq.Expressions.MethodCallExpression, bool, Mangos.Condition>;

namespace Mangos
{
    [DebuggerNonUserCode]
    public class Parser
    {
        private static Parser _parser = new Parser();

        #region Condition parse preparation

        private class ParseInfo
        {
            public MethodInfo Method { get; set; }

            public bool Negatable { get; set; }

            public ParseDelegate Parse;
        }

        private Dictionary<string, ParseInfo> _parseInfoLookUp;

        private Parser()
        {
            _parseInfoLookUp = new Dictionary<string, ParseInfo>();

            foreach (var method in typeof(ConditionFacade).GetMethods().Where(m => IsCondition(m)))
                Setup(method);
        }

        private bool IsCondition(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(ConditionAttribute), false).Cast<ConditionAttribute>().ToArray();
            return attributes.Length != 0;
        }

        private bool IsNegatable(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(ConditionAttribute), false).Cast<ConditionAttribute>().ToArray();
            return attributes.Length != 0 && attributes[0].Negatable;
        }

        private void Setup(MethodInfo method)
        {
            var parseMethodName = "Parse" + method.Name;
            var parseMethod = typeof(ConditionFacade).GetMethod(parseMethodName, BindingFlags.NonPublic | BindingFlags.Static);

            if (parseMethod == null)
                throw new MissingMethodException(typeof(ConditionFacade).Name + "." + parseMethodName);

            var parseDelegate = (ParseDelegate)ParseDelegate.CreateDelegate(typeof(ParseDelegate), parseMethod);

            var info = new ParseInfo()
            {
                Method = method,
                Negatable = IsNegatable(method),
                Parse = parseDelegate
            };

            _parseInfoLookUp[method.Name] = info;
        }

        #endregion

        private Condition Parse(MethodCallExpression expr, bool not)
        {
            var parseInfo = _parseInfoLookUp[expr.Method.Name];

            if (not && !parseInfo.Negatable)
                throw new InvalidOperationException("Cannot apply operator ! to " + expr.Method.Name);

            var condition = _parseInfoLookUp[expr.Method.Name].Parse(expr, not);

            Console.WriteLine(condition);

            return condition;
        }

        #region Expression tree traversal

        public void Parse(Expression expression)
        {
            switch (expression.NodeType)
            {
            case ExpressionType.Call:
                ParseCall(expression as MethodCallExpression);
                break;

            case ExpressionType.OrElse:
            case ExpressionType.Or:
                ParseOr(expression as BinaryExpression);
                break;

            case ExpressionType.AndAlso:
            case ExpressionType.And:
                ParseAnd(expression as BinaryExpression);
                break;

            case ExpressionType.Not:
                var expr = ((UnaryExpression)expression).Operand as MethodCallExpression;
                ParseCall(expr, true);
                break;

            default:
                Console.WriteLine(expression.NodeType);
                break;
            }
        }

        private void ParseCall(MethodCallExpression expression, bool not = false)
        {
            Parse(expression, not);
        }

        private void ParseOr(BinaryExpression expression)
        {
            Parse(expression.Left);
            Console.WriteLine(" or ");
            Parse(expression.Right);
        }

        private void ParseAnd(BinaryExpression expression)
        {
            Parse(expression.Left);
            Console.WriteLine(" and ");
            Parse(expression.Right);
        }

        #endregion

        public static void Parse(Expression<Func<ConditionFacade, bool>> expression)
        {
            _parser.Parse(expression.Body);
        }
    }
}
