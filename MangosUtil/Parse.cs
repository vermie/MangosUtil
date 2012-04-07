using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ParseDelegate = System.Func<System.Linq.Expressions.MethodCallExpression, bool, Mangos.Condition>;

namespace Mangos
{
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

        private ICondition Parse(MethodCallExpression expr, bool not)
        {
            var parseInfo = _parseInfoLookUp[expr.Method.Name];

            if (not && !parseInfo.Negatable)
                throw new InvalidOperationException("Cannot apply operator ! to " + expr.Method.Name);

            var condition = _parseInfoLookUp[expr.Method.Name].Parse(expr, not);

            return condition;
        }

        #region Expression tree traversal

        private ICondition Parse(Expression expression)
        {
            switch (expression.NodeType)
            {
            case ExpressionType.Call:
                return ParseCall(expression as MethodCallExpression);

            case ExpressionType.OrElse:
            case ExpressionType.Or:
                return ParseOr(expression as BinaryExpression);

            case ExpressionType.AndAlso:
            case ExpressionType.And:
                return ParseAnd(expression as BinaryExpression);

            case ExpressionType.Not:
                var expr = ((UnaryExpression)expression).Operand as MethodCallExpression;
                return ParseCall(expr, true);

            default:
                throw new InvalidOperationException(expression.NodeType + " is an unsupported expression type.");
            }
        }

        private ICondition ParseCall(MethodCallExpression expression, bool not = false)
        {
            return Parse(expression, not);
        }

        private ICondition ParseOr(BinaryExpression expression)
        {
            return new OrCondition(Parse(expression.Left), Parse(expression.Right));
        }

        private ICondition ParseAnd(BinaryExpression expression)
        {
            return new AndCondition(Parse(expression.Left), Parse(expression.Right));
        }

        #endregion

        public static ICondition Parse(ref int id, Expression<Func<ConditionFacade, bool>> expression)
        {
            var expr = _parser.Parse(expression.Body);

            foreach (var condition in expr.Conditions)
                condition.Id = id++;

            return expr;
        }
    }
}
