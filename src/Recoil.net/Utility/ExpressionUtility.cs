using System.Linq.Expressions;
using System.Reflection;

namespace RecoilNet.Utility
{
	internal static class ExpressionUtility
	{
		/// <summary>
		/// Gets the property path of the given expression
		/// </summary>
		public static string GetPropertyPath<T>(Expression<T> expression)
		{
			ArgumentNullException.ThrowIfNull(expression);

			if (expression.Body is not MemberExpression expressionMember)
			{
				throw new ArgumentException($"The expression must just be a property accessor like '() => MyProperty'. The value received was {expression.Body}");
			}

			MemberInfo memberInfo = expressionMember.Member;

			if (memberInfo == null)
			{
				throw new ArgumentNullException("Unable to acecss the member from the expression");
			}

			return $"{memberInfo.DeclaringType?.Name ?? "Unknown"}.{memberInfo.Name}";
		}
	}
}
