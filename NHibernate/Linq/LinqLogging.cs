using System.Linq.Expressions;
using NHibernate.Linq.Visitors;
using NHibernate.Proxy;
using NHibernate.Util;

namespace NHibernate.Linq
{
	internal static class LinqLogging
	{
		private static readonly IInternalLogger Log = LoggerProvider.LoggerFor("NHibernate.Linq");

		/// <summary>
		/// If debug logging is enabled, log a string such as "msg: expression.ToString()".
		/// </summary>
		internal static void LogExpression(string msg, Expression expression)
		{
			if (Log.IsDebugEnabled)
			{
				// If the expression contains NHibernate proxies, those will be initialized
				// when we call ToString() on the exception. The string representation is
				// generated by a class internal to System.Linq.Expression, so we cannot
				// actually override that logic. Circumvent it by replacing such ConstantExpressions
				// with ParameterExpression, having their name set to the string we wish to display.
				var visitor = new ProxyReplacingExpressionVisitor();
				var preparedExpression = visitor.Visit(expression);

				Log.DebugFormat("{0}: {1}", msg, preparedExpression.ToString());
			}
		}


		/// <summary>
		/// Replace all occurrences of ConstantExpression where the value is an NHibernate
		/// proxy with a ParameterExpression. The name of the parameter will be a string
		/// representing the proxied entity, without initializing it.
		/// </summary>
		private class ProxyReplacingExpressionVisitor : NhExpressionVisitor
		{
			// See also e.g. Remotion.Linq.Clauses.ExpressionVisitors.FormattingExpressionTreeVisitor
			// for another example of this technique.

			protected override Expression VisitConstant(ConstantExpression expression)
			{
				if (expression.Value.IsProxy())
					return Expression.Parameter(expression.Type, ObjectHelpers.IdentityToString(expression.Value));

				return base.VisitConstant(expression);
			}
		}
	}
}