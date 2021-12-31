using RecoilNet.Interfaces;
using RecoilNet.State;
using System.Diagnostics;

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
		private class Builder : ISelectorBuilder
		{
			private readonly IRecoilStore? m_store;
			private readonly Selector<T> m_selector;

			public Builder(IRecoilStore? store, Selector<T> selector)
			{
				m_selector = selector;
				m_store = store;
			}

			public TValue? Value<TValue>(Atom<TValue> atom)
			{
				ArgumentNullException.ThrowIfNull(atom);

				atom.AddDependent(m_selector);
				return atom.GetValue(m_store);
			}

			public TValue? Value<TValue>(Selector<TValue> selector)
			{
				ArgumentNullException.ThrowIfNull(selector);

				selector.AddDependent(m_selector);
				return selector.GetValue(m_store);
			}
		}

		public delegate T? GetValueDelegate(ISelectorBuilder builder);

		/// <summary>
		/// Ges
		/// </summary>
		public GetValueDelegate Get { get; }

		/// <summary>
		/// Initializes a new instance of a selector
		/// </summary>
		/// <param name="get"></param>
		public Selector(string key, GetValueDelegate get) : base(key, false)
		{
			ArgumentNullException.ThrowIfNull(get);
			Get = get;
		}


		/// <inheritdoc cref="RecoilValue{T}"/>
		public override T? GetValue(IRecoilStore? recoilStore)
		{
			Builder builder = new Builder(recoilStore, this);
			return Get(builder);
		}
	}
}
