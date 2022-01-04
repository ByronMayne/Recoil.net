using RecoilNet.Effects;
using RecoilNet.State;
using RecoilNet.Utility;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace RecoilNet
{

	/// <summary>
	/// Atoms are units of state. They're updateable and subscribable: 
	/// when an atom is updated, each subscribed component is re-rendered with the new value. 
	/// </summary>

	public class Atom<T> : RecoilValue<T>
	{

		private readonly Func<IRecoilStore?, Task<T?>> m_defaultValueProvider;

		/// <summary>
		/// Gets the list of effects that are applied to the atom
		/// </summary>
		public IReadOnlyList<IAtomEffect<T>> Effects { get; }

		/// <summary>
		/// Initializes a new instance of an Atom using whatever the default value is
		/// </summary>
		/// <param name="key"></param>
		public Atom(string key, params IAtomEffect<T>[] effects) : this(key, default(T))
		{
			Effects = effects ?? Array.Empty<IAtomEffect<T>>();
		}

		/// <summary>
		/// Initializes a new atom using a set defalut value
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		public Atom(string key, T? defaultValue, params IAtomEffect<T>[] effects) : base(key, true)
		{
			Effects = effects ?? Array.Empty<IAtomEffect<T>>();
			m_defaultValueProvider = (_) => Task.FromResult(defaultValue);
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		public Atom(string key, Atom<T> defaultValue, params IAtomEffect<T>[] effects) : this(key, defaultRecoilValue: defaultValue, effects)
		{ }

		/// <inheritdoc cref="RecoilValue{T}"/>
		public Atom(string key, Selector<T> defaultValue, params IAtomEffect<T>[] effects) : this(key, defaultRecoilValue: defaultValue, effects)
		{ }

		internal Atom(string key, RecoilValue<T> defaultRecoilValue, params IAtomEffect<T>[] effects) : base(key, true)
		{
			ArgumentNullException.ThrowIfNull(defaultRecoilValue);

			Effects = effects ?? Array.Empty<IAtomEffect<T>>();
			m_defaultValueProvider = defaultRecoilValue.GetValueAsync;
			m_dependents.Add(defaultRecoilValue);
		}

		/// <summary>
		/// Gets the value of a <see cref="RecoilValue"/> from the instance of the store. 
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="recoilStore">The store instance to attempt to fetch it from</param>
		/// <returns>The value</returns>
		public override async Task<T?> GetValueAsync(IRecoilStore? recoilStore)
		{
			if (recoilStore == null || !recoilStore.TryGetValue<T>(this, out T? storeValue))
			{
				return await m_defaultValueProvider(recoilStore);
			}
			return storeValue;
		}

		/// <inheritdoc cref="RecoilValue"/>
		public override async Task SetValueAsync(IRecoilStore? recoilStore, T? value)
		{
			if (recoilStore == null)
			{
				return;
			}

			if (!IsMutable)
			{
				throw ErrorFactory.AssigningValueToNonMutableType(this);
			}
			await recoilStore.SetValueAsync<T>(this, value);
		}

		/// <inheritdoc cref="RecoilValue"/>
		internal override string RenderDebug()
				=> $"Atom<{typeof(T).Name}>: {Key}";


	}
}