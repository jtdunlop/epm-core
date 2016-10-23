using System;
using System.Linq.Expressions;

namespace DBSoft.EPM.UI
{
    public static class ExpressionHelper
	{
	    public static string GetMemberName(Expression expression)
	    {
	        while (true)
	        {
	            switch (expression.NodeType)
	            {
	                case ExpressionType.Lambda:
	                    var lambdaExpression = (LambdaExpression) expression;
	                    expression = lambdaExpression.Body;
	                    continue;
	                case ExpressionType.MemberAccess:
	                    var memberExpression = (MemberExpression) expression;
	                    var supername = GetMemberName(memberExpression.Expression);
	                    return String.IsNullOrEmpty(supername) ? memberExpression.Member.Name : String.Concat(supername, '.', memberExpression.Member.Name);
	                case ExpressionType.Call:
	                    var callExpression = (MethodCallExpression) expression;
	                    return callExpression.Method.Name;
	                case ExpressionType.Convert:
	                    var unaryExpression = (UnaryExpression) expression;
	                    expression = unaryExpression.Operand;
	                    continue;
	                case ExpressionType.Parameter:
	                case ExpressionType.Constant:
	                    return String.Empty;
	                default:
	                    throw new ArgumentException("The expression is not a member access or method call expression");
	            }
	        }
	    }
	}
}
