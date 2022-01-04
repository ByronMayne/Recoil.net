using RecoilNet.Utility;
using System.Linq.Expressions;
using static RecoilNet.Atom;

namespace RecoilNet
{
	/// <summary>
	/// Just contains helper methods for working with <see cref="Atom{T}"/>s
	/// </summary>
	public static class Atom
	{
		public delegate Atom<T>? PropertyAccessor<T>();

		/// <summary>
		/// Creates a new Atom{T} with a default being whatever the default of T is. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created atom</returns>
		public static Atom<T> Create<T>(Expression<PropertyAccessor<T>> expression)
			=> Create<T>(expression, default(T));



		/// <summary>
		/// Creates a new Atom{T} with a defined hard coded default value. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created atom</returns>
		public static Atom<T> Create<T>(Expression<PropertyAccessor<T>> expression, T? defaultValue)
		{
			ArgumentNullException.ThrowIfNull(expression);
			string path = ExpressionUtility.GetPropertyPath(expression);
			return new Atom<T>(path, defaultValue);
		}

		/// <summary>
		/// Creates a new Atom{T} with a default value of another atom. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created atom</returns>
		public static Atom<T> Create<T>(Expression<PropertyAccessor<T>> expression, Atom<T> defaultValue)
			=> CreateRecoilValueInternal<T>(expression, defaultValue);

		/// <summary>
		/// Creates a new Atom{T} with a default value of a selector. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created atom</returns>
		public static Atom<T> Create<T>(Expression<PropertyAccessor<T>> expression, Selector<T> defaultValue)
			=> CreateRecoilValueInternal<T>(expression, defaultValue);

		/// <summary>
		/// Creates a new <see cref="Atom{T}"/> instance using an expression with
		/// a recoil value as the default.
		/// </summary>
		private static Atom<T> CreateRecoilValueInternal<T>(Expression<PropertyAccessor<T>> expression, RecoilValue<T> defaultValue)
		{
			string path = ExpressionUtility.GetPropertyPath(expression);
			return new Atom<T>(path, defaultValue); ;
		}
	}
}