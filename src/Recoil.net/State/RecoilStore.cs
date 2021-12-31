using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet.State
{
	/// <summary>
	/// THe 
	/// </summary>
	public class RecoilStore : IRecoilStore
	{
		private record CallbackTarget(WeakReference Target, MethodInfo Method)
		{
			public void Invoke(IList<RecoilValue> changedValues)
			{
				Method.Invoke(Target.Target, new object[] { changedValues });
			}
		}

		private readonly IDictionary<string, RecoilValue> m_objects;
		private readonly IDictionary<string, object?> m_values;
		private readonly IList<CallbackTarget> m_valueCallbacks;

		public event RecoilValueChangedDelegate OnValueChanged
		{
			add => m_valueCallbacks.Add(new CallbackTarget(new WeakReference(value.Target), value.Method));
			remove
			{
				for (int i = m_valueCallbacks.Count - 1; i >= 0; i--)
				{
					CallbackTarget target = m_valueCallbacks[i];



					if (target.Method == value.Method && target.Target.IsAlive && target.Target == value.Target)
					{
						m_valueCallbacks.RemoveAt(i);
						return;
					}
				}
			}
		}

		public RecoilStore()
		{
			m_objects = new Dictionary<string, RecoilValue>();
			m_values = new Dictionary<string, object?>();
			m_valueCallbacks = new List<CallbackTarget>();

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

			if (HasValue<T>(recoilValue))
			{
				T? previousValue = GetValue<T>(recoilValue);

				if (EqualityComparer<T>.Default.Equals(previousValue, value))
				{
					// Values are already equal
					return;
				}

			}

			// Set it 
			m_values[recoilValue.Key] = value;

			List<RecoilValue> changedValues = new List<RecoilValue>();
			GetChangedValues(recoilValue, changedValues);

			foreach (CallbackTarget? listener in m_valueCallbacks)
			{
				listener.Invoke(changedValues);
			}
		}


		private static void GetChangedValues(RecoilValue current, List<RecoilValue> dependents)
		{
			dependents.Add(current);

			if (current.Dependents.Count > 0)
			{
				foreach (RecoilValue dependent in current.Dependents)
				{
					GetChangedValues(dependent, dependents);
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
			=> new RecoilState<T>(atom, this);

		/// <inheritdoc cref="IRecoilStore"/>
		public RecoilState<T> UseState<T>(Selector<T> selector)
			=> new RecoilState<T>(selector, this);
	}
}
