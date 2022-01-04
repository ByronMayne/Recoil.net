using RecoilNet.Utility;
using System.Linq.Expressions;
using System.Reflection;

namespace RecoilNet
{
	/// <summary>
	/// Contains helper functions for working with selectors
	/// </summary>
	public static class Selector
	{
		public delegate Selector<T>? PropertyAccessor<T>();

		/// <summary>
		/// Creates a new readonly Selector{T}. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created selector</returns>
		public static Selector<T> Create<T>(Expression<PropertyAccessor<T>> expression, Selector<T>.ValueGetter getter)
		{
			ArgumentNullException.ThrowIfNull(expression);
			ArgumentNullException.ThrowIfNull(getter);
			string path = ExpressionUtility.GetPropertyPath(expression);
			return new Selector<T>(path, getter);
		}

		/// <summary>
		/// Creates a new read and writable Selector{T}. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created selector</returns>
		public static Selector<T> Create<T>(Expression<PropertyAccessor<T>> expression, Selector<T>.ValueGetter getter, Selector<T>.ValueSetter setter)
		{
			ArgumentNullException.ThrowIfNull(expression);
			ArgumentNullException.ThrowIfNull(getter);
			ArgumentNullException.ThrowIfNull(setter);

			string path = ExpressionUtility.GetPropertyPath(expression);
			return new Selector<T>(path, getter, setter);
		}
	}
}
