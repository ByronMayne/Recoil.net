

using System;
using RecoilNet.Utility;
using RecoilNet.Values;
using RecoilNet.Diagnostics;
using RecoilNet.Effects;

using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace RecoilNet
{
	public static partial class AtomFamily
	{
		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, ValueProvider<T>.Default, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, T constant,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(constant);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<T> factory,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(factory);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Task<T> asyncValue,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncValue);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<Task<T>> asyncFactory,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncFactory);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<IRecoilStore, T> recoilValue,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(recoilValue);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<IRecoilStore, Task<T>> asyncRecoilValue,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(asyncRecoilValue);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Func<TParam, T?> keyedFactory,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(keyedFactory);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, AtomFamily<T, TParam> atomFamily,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(atomFamily);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, AtomFamily<T, TParam> atomFamily, TParam fixedValue,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(atomFamily);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Atom<T> atom,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(atom);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}

		public static AtomFamily<T,TParam> Create<T,TParam>(Expression<Func<Atom<T>>> expression, Selector<T> selector,
			IPrimitiveEffect<T>[]? effects = null,
			[CallerFilePath] string callerFilePath = "",
			[CallerMemberName] string callerMemberName = "",
			[CallerLineNumber] int callerLineNumber = 0) where TParam : notnull
		{
			Guard.NotNull(expression, nameof(expression));
			effects ??= Array.Empty<IPrimitiveEffect<T>>();
			string path = ExpressionUtility.GetPropertyPath(expression);
			Key key = Key.From(path);
			CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
			ValueProvider<T> valueProvider = ValueProvider.Create(selector);
			AtomFamilyNode<T,TParam> node = new AtomFamilyNode<T,TParam>(key, creatorInfo, valueProvider, effects);
			return node.Get;
		}


	}
}