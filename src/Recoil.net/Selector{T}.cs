using RecoilNet.Interfaces;
using RecoilNet.Providers;
using RecoilNet.State;
using RecoilNet.Utility;
using System.Diagnostics;
using System.Reflection;

namespace RecoilNet
{
	/// <summary>
	/// A selector represents a piece of derived state. You can think of 
	/// derived state as the output of passing state to a pure function 
	/// that modifies the given state in some way.
	/// </summary>
	[DebuggerDisplay("Selector<{typeof(T).Name}>: {Key}")]
	public class Selector<T> : RecoilValue<T>
	{
		public delegate Task<T?> ValueGetter(IValueProvider asyncBuilder);
		public delegate Task ValueSetter(IRecoilStore provider, T? Value);

		private readonly ValueGetter m_getter;
		private readonly ValueSetter? m_setter;

		/// <summary>
		/// Initializes a new instance of a selector that uses an sync method
		/// </summary>
		/// <param name="getter">The method to get the value</param>
		public Selector(string key, ValueGetter getter) : base(key, false)
		{
			ArgumentNullException.ThrowIfNull(getter);
			m_getter = getter;
		}


		public Selector(string key, ValueGetter getter, ValueSetter setter) : base(key, true)
		{
			ArgumentNullException.ThrowIfNull(getter);
			ArgumentNullException.ThrowIfNull(getter);
			m_getter = getter;
			m_setter = setter;
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		public override void SetValue(IRecoilStore? recoilStore, T? value)
		{
			if (recoilStore == null)
			{
				return;
			}

			if (!IsMutable || m_setter == null)
			{
				throw ErrorFactory.AssigningValueToNonMutableType(this);
			}

			m_setter(recoilStore, value);
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		public override async Task<T?> GetValueAsync(IRecoilStore? recoilStore)
		{
			ValueProvider<T> valueProvider = new ValueProvider<T>(recoilStore, this);
			return await m_getter(valueProvider);
		}
	}
}
