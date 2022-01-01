using RecoilNet.Interfaces;
using RecoilNet.Providers;
using RecoilNet.State;
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
		public delegate Task<T?> ValueBuilder(IValueProvider asyncBuilder);

		private readonly ValueBuilder m_valueBuilder;

		/// <summary>
		/// Initializes a new instance of a selector that uses an sync method
		/// </summary>
		/// <param name="builder">The method to get the value</param>
		public Selector(string key, ValueBuilder builder) : base(key, false)
		{
			ArgumentNullException.ThrowIfNull(builder);
			m_valueBuilder = builder;
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		public override async Task<T?> GetValueAsync(IRecoilStore? recoilStore)
		{
			ValueProvider<T> valueProvider = new ValueProvider<T>(recoilStore, this);
			return await m_valueBuilder(valueProvider);
		}
	}
}
