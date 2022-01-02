using RecoilNet.Effects;

namespace RecoilNet.State
{
	/// <summary>
	/// THe 
	/// </summary>
	public sealed class RecoilStore : IRecoilStore
	{
		private readonly IDictionary<string, RecoilValue> m_objects;
		private readonly IDictionary<string, object?> m_values;


		/// <inheritdoc cref="IRecoilStore"/>
		public IList<RecoilState> States { get; }


		public RecoilStore()
		{
			m_objects = new Dictionary<string, RecoilValue>();
			m_values = new Dictionary<string, object?>();
			States = new List<RecoilState>();

		}

		/// <inheritdoc cref="IRecoilStore"/>
		public RecoilState<T> GetState<T>(RecoilValue<T> recoilValue)
		{
			ArgumentNullException.ThrowIfNull(recoilValue);
			TrackObject(recoilValue);
			return new RecoilState<T>(recoilValue, this);
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public void SetValue<T>(RecoilValue<T> recoilValue, T? value)
		{
			TrackObject(recoilValue);

			T? previousValue = default(T);

			if (HasValue<T>(recoilValue))
			{
				previousValue = GetValue<T>(recoilValue);

				if (EqualityComparer<T>.Default.Equals(previousValue, value))
				{
					// Values are already equal
					return;
				}

			}

			// Invoke effects 
			if (recoilValue is Atom<T> asAtom)
			{
				foreach (IAtomEffect<T> effect in asAtom.Effects)
				{
					effect.OnSet(value, previousValue, false);
				}
			}

			// Set it 
			m_values[recoilValue.Key] = value;

			// Push update notifications onto another thread
			Task.Run(() => NotifyListenersAsync(recoilValue));
		}

		private async void NotifyListenersAsync(RecoilValue rootChange)
		{
			HashSet<RecoilValue> dependents = new HashSet<RecoilValue>();
			GetDepdendents(rootChange, dependents);

			foreach (RecoilState state in States)
			{
				if (rootChange == state.RecoilValue)
				{
					await state.ValueChangedAsync(this, m_values[rootChange.Key]);
				}
				else if (dependents.Contains(state.RecoilValue))
				{
					await state.DependentChangedAsync(this, rootChange);
				}
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
		public T? GetValue<T>(RecoilValue<T> recoilObject)
		{
			ArgumentNullException.ThrowIfNull(recoilObject);

			TrackObject(recoilObject);
			return HasValue(recoilObject)
				? (T?)m_values[recoilObject.Key]
				: throw new KeyNotFoundException($"Unable to find value for the atom '{recoilObject.Key}'");
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public bool HasValue<T>(RecoilValue<T>? recoilObject)
		{
			return recoilObject != null && m_values.ContainsKey(recoilObject.Key);
		}

		/// <inheritdoc cref="IRecoilStore"/>
		public bool TryGetValue<T>(RecoilValue<T> recoilObject, out T? value)
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
		public void ResetValue<T>(RecoilValue<T> recoilObject)
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
	}
}
