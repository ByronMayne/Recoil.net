using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet.Components
{
	/// <summary>
	/// Defines a custom component that can be added to the store to allow you to hook into
	/// callbacks
	/// </summary>
	public interface IStoreComponent : IDisposable
	{
		/// <summary>
		/// Raised when the store has received the component and can be configured.
		/// </summary>
		/// <param name="store">The store the component was added too</param>
		void Initialize(IRecoilStore store);

		/// <summary>
		/// Raised whenever a new <see cref="RecoilValue{T}"/> is link to a store instance.
		/// </summary>
		/// <typeparam name="T">The value type of the state</typeparam>
		/// <param name="state">The state instance that was linked</param>
		/// <param name="recoilStore">The store it was linked too</param>
		void OnStateAdded<T>(RecoilState<T> state, IRecoilStore recoilStore);

		/// <summary>
		/// Raised whenever a new <see cref="RecoilValue{T}"/> is unlinked to a store instance
		/// </summary>
		/// <typeparam name="T">The value type of the store</typeparam>
		/// <param name="state">The state that was unlinked</param>
		/// <param name="recoilStore">The store it was removed from</param>
		void OnStateRemoved<T>(RecoilState<T> state, IRecoilStore recoilStore);

		/// <summary>
		/// Raised whenever an atom is assigned a new value to the given store.
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="recoilStore">The store that holds the new value</param>
		/// <param name="changedAtom">The atom that was changed</param>
		/// <param name="dependents">The list of atoms and selectors that are dependent on the value being changed.</param>
		void OnValueChanged<T>(RecoilStore recoilStore, Atom<T> changedAtom, HashSet<RecoilValue> dependents);
	}
}
