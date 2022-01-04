using RecoilNet.Components;
using RecoilNet.Effects;
using System;

namespace RecoilNet.State
{
	/// <summary>
	/// THe 
	/// </summary>
	public sealed class RecoilStore : IRecoilStore, IDisposable
	{
		private readonly IDictionary<string, RecoilValue> m_objects;
		private readonly IDictionary<string, object?> m_values;
		private readonly IReadOnlyList<IStoreComponent> m_components;

		private static readonly List<WeakReference<RecoilStore>> s_stores;
		private List<RecoilState> m_states;

		/// <inheritdoc cref="IRecoilStore"/>
		public int Id { get; }

		/// <summary>
		/// Contains a weak reference to all the RecoilStores that have been defined.
		/// </summary>
		internal static IReadOnlyList<WeakReference<RecoilStore>> Stores
			=> s_stores;

		static RecoilStore()
		{
			s_stores = new List<WeakReference<RecoilStore>>();
		}

		/// <summary>
		/// Creates a new recoil store without any custom components 
		/// </summary>
		public RecoilStore() : this(Array.Empty<IStoreComponent>())
		{ }

		/// <summary>
		/// Creats a new recoil store with a set list of components 
		/// </summary>
		/// <param name="components"></param>
		public RecoilStore(IEnumerable<IStoreComponent> components)
		{
			ArgumentNullException.ThrowIfNull(components);

			m_components = components.ToArray();
			m_objects = new Dictionary<string, RecoilValue>();
			m_values = new Dictionary<string, object?>();
			m_states = new List<RecoilState>();
			s_stores.Add(new WeakReference<RecoilStore>(this));
			Id = s_stores.Count;

			foreach (IStoreComponent component in m_components)
			{
				component.Initialize(this);
			}
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public void AddState<T>(RecoilState<T> state)
		{
			m_states.Add(state);

			foreach (IStoreComponent component in m_components)
			{
				component.OnStateAdded<T>(state, this);
			}
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public void RemoveState<T>(RecoilState<T> state)
		{
			m_states.Remove(state);

			foreach (IStoreComponent component in m_components)
			{
				component.OnStateRemoved<T>(state, this);
			}
		}

		public async Task SetValueAsync<T>(Atom<T> atom, T? value)
		{
			SetValueInternal(atom, value);
			await NotifyListenersAsync<T>(atom, value);
		}


		/// <inheritdoc cref="IRecoilStore"/>
		public void SetValue<T>(Atom<T> atom, T? value)
		{
			SetValueInternal(atom, value);
			// Notify on background job 
			Task.Run(() => NotifyListenersAsync<T>(atom, value));
		}

		private void SetValueInternal<T>(Atom<T> atom, T? value)
		{
			TrackObject(atom);

			T? previousValue = default(T);

			if (HasValue<T>(atom))
			{
				previousValue = GetValue<T>(atom);

				if (EqualityComparer<T>.Default.Equals(previousValue, value))
				{
					// Values are already equal
					return;
				}

			}

			// Invoke effects 
			if (atom is Atom<T> asAtom)
			{
				foreach (IAtomEffect<T> effect in asAtom.Effects)
				{
					effect.OnSet(value, previousValue, false);
				}
			}

			// Set it 
			m_values[atom.Key] = value;
		}

		private async Task NotifyListenersAsync<T>(Atom<T> changedAtom, T? value)
		{
			HashSet<RecoilValue> dependents = new HashSet<RecoilValue>();
			GetDepdendents(changedAtom, dependents);

			foreach (RecoilState state in m_states)
			{
				if (changedAtom == state.RecoilValue)
				{
					await state.ValueChangedAsync(this, m_values[changedAtom.Key]);
				}
				else if (dependents.Contains(state.RecoilValue))
				{
					await state.DependentChangedAsync(this, changedAtom);
				}
			}

			// We invoke this after because `dependents` could be modifed by external plugins and
			// at this point we don't care anymore 
			foreach (IStoreComponent component in m_components)
			{
				component.OnValueChanged<T>(this, changedAtom, value, dependents);
			}
		}


		private static void GetDepdendents(RecoilValue current, HashSet<RecoilValue> dependents)
		{
			if (current.Dependents.Count > 0)
			{
				foreach (RecoilValue dependent in current.Dependents)
				{
					dependents.Add(dependent);

					GetDepdendents(dependent, dependents);
				}
			}
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public T? GetValue<T>(Atom<T> recoilObject)
		{
			ArgumentNullException.ThrowIfNull(recoilObject);

			TrackObject(recoilObject);
			return HasValue(recoilObject)
				? (T?)m_values[recoilObject.Key]
				: throw new KeyNotFoundException($"Unable to find value for the atom '{recoilObject.Key}'");
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public bool HasValue<T>(Atom<T>? recoilObject)
		{
			return recoilObject != null && m_values.ContainsKey(recoilObject.Key);
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public bool TryGetValue<T>(Atom<T> recoilObject, out T? value)
		{
			ArgumentNullException.ThrowIfNull(recoilObject);
			value = default;

			if (HasValue(recoilObject))
			{
				value = GetValue(recoilObject);
				return true;
			}
			return false;
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public void ResetValue<T>(Atom<T> recoilObject)
		{
			TrackObject(recoilObject);
			if (HasValue(recoilObject))
			{
				m_values.Remove(recoilObject.Key);
			}
		}

		private void TrackObject(RecoilValue recoilObject)
		{
			if (m_objects.ContainsKey(recoilObject.Key))
			{
				RecoilValue otherObject = m_objects[recoilObject.Key];

				if (!ReferenceEquals(otherObject, recoilObject))
				{
					string error = $"The key '{recoilObject.Key}' for the {recoilObject.GetType().Name} already" +
						$"exists for other Rocoil object {otherObject.GetType().Name}. Each key can only be used " +
						" once but the instance can be shared.";
					throw new InvalidOperationException(error);
				}
			}
			else
			{
				m_objects[recoilObject.Key] = recoilObject;
			}
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public RecoilState<T> UseState<T>(Atom<T> atom)
		{
			return new RecoilState<T>(atom, this);
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public RecoilState<T> UseState<T>(Selector<T> selector)
		{
			return new RecoilState<T>(selector, this);
		}

		void IDisposable.Dispose()
		{
			m_values.Clear();
			s_stores.Clear();
			m_objects.Clear();
			lock (s_stores)
			{
				for (int i = 0; i < m_states.Count; i++)
				{
					WeakReference<RecoilStore>? weakStore = Stores[i];
					if (weakStore.TryGetTarget(out RecoilStore? store))
					{
						if (store == this)
						{
							s_stores.RemoveAt(i);
						}
					}
					else
					{
						s_stores.RemoveAt(i);
					}
				}
			}
		}
	}
}
