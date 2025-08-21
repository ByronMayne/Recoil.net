

using System;
using RecoilNet;
using RecoilNet.Utility;
using RecoilNet.Values;
using System.Linq.Expressions;
using RecoilNet.State;
using System.Runtime.CompilerServices;

namespace Recoil
{
	public static partial class AtomFamily
	{
		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			return new AtomF<T>(path);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, T constant,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(constant);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<T> factory,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(factory);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Task<T> asyncValue,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncValue);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<Task<T>> asyncFactory,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncFactory);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<IRecoilStore, T> recoilValue,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(recoilValue);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<IRecoilStore, Task<T>> asyncRecoilValue,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncRecoilValue);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<TParam, T?> keyedFactory,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) 
				where TParam : notnull
		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(keyedFactory);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, AtomFamily<T, TParam> atomFamily,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) 
				where TParam : notnull
		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(atomFamily);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, AtomFamily<T, TParam> atomFamily, TParam fixedValue,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) 
				where TParam : notnull
		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(atomFamily);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Atom<T> atom,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(atom);
			return new Atom<T>(path, valueProvider);
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Selector<T> selector,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0)		{
			Gaurd.NotNull(expression, nameof(expression));
			string path = ExpressionUtility.GetPropertyPath(expression);
			ValueProvider<T> valueProvider = ValueProvider.Create(selector);
			return new Atom<T>(path, valueProvider);
		}


	}
}