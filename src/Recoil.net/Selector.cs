using RecoilNet.Diagnostics;
using RecoilNet.Utility;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
		public static Selector<T> Create<T>(Expression<PropertyAccessor<T>> expression, 
			Selector<T>.ValueGetter getter,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
		{
			Guard.NotNull(expression);
            Guard.NotNull(getter);
			Key key = Key.From(expression);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
            return new Selector<T>(key, creatorInfo, getter);
		}

		/// <summary>
		/// Creates a new read and writable Selector{T}. The key will be auto generated
		/// based of the $"{ClassName}.{PropertyName}" which should keep it unique.
		/// </summary>
		/// <typeparam name="T">The value type of the property</typeparam>
		/// <param name="expression">The expression to access the property</param>
		/// <returns>The created selector</returns>
		public static Selector<T> Create<T>(Expression<PropertyAccessor<T>> expression, 
			Selector<T>.ValueGetter getter, 
			Selector<T>.ValueSetter setter,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
		{
            Guard.NotNull(expression);
            Guard.NotNull(getter);
            Guard.NotNull(setter);
            Key key = Key.From(expression);
            CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
            return new Selector<T>(key, creatorInfo, getter, setter);
        }
	}
}
