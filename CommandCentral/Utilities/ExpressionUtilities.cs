﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CommandCentral.Utilities
{
    public static class ExpressionUtilities
    {
        /// <summary>
        /// For the given property of the given type, returns the name of that property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            return GetProperty(expression).Name;
        }

        /// <summary>
        /// For the given property of a given type, returned the member info of that property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetProperty<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            if (expression.Body is MemberExpression memberExp)
                return memberExp.Member;

            // for unary types like datetime or guid
            if (!(expression.Body is UnaryExpression unaryExp))
                throw new ArgumentException(
                    $"'{nameof(expression)}' should be a member expression or a method call expression.",
                    nameof(expression));
            
            memberExp = unaryExp.Operand as MemberExpression;
            if (memberExp != null)
                return memberExp.Member;

            throw new ArgumentException($"'{nameof(expression)}' should be a member expression or a method call expression.", nameof(expression));
        }

        public static Expression<Func<T, bool>> NullSafeOr<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            if (expr2 == null)
                throw new ArgumentNullException(nameof(expr2));

            return expr1 == null 
                ? expr2 
                : expr1.Or(expr2);
        }

        public static Expression<Func<T, bool>> NullSafeAnd<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            if (expr2 == null)
                throw new ArgumentNullException(nameof(expr2));

            return expr1 == null 
                ? expr2 
                : expr1.And(expr2);
        }

        //x => x.LastName == "test"
        //x => x.FirstName == "other test"
        //x => x.LastName == test || x.FirstName == "other test"
        private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
