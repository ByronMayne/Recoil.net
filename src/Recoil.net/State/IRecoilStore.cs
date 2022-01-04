namespace RecoilNet.State
{
	public delegate Task RecoilValueChangedDelegate(IRecoilStore store, IList<RecoilValue> changedValues);

	/// <summary>
	/// Defines an object that can get and set the value of a atom or selector
	/// </summary>
	public interface IRecoilStore
	{
		/// <summary>
		/// Gets the list of states that are being tracked by this store
		/// </summary>
		IList<RecoilState> States { get; }

		/// <summary>
		/// Gets the value of the <see cref="RecoilValue{T}"/> 
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="atom">The atom to set</param>
		/// <returns>The atoms current value or of none is defined the default value</returns>
		T? GetValue<T>(Atom<T> atom);

		/// <summary>
		/// Gets if there is currently a value assigned to an atom in this store.
		/// </summary>
		/// <typeparam name="T">The type of the atom</typeparam>
		/// <param name="atom">The atom itself</param>
		/// <returns>True if a value exists</returns>
		bool HasValue<T>(Atom<T>? atom);

		/// <summary>
		/// Attempts to get the value from a store
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="atom">The object to fetch the value from</param>
		/// <param name="value">The value that was found or non if no value was in the store</param>
		/// <returns>True if the value was found otherwise false</returns>
		bool TryGetValue<T>(Atom<T> atom, out T? value);

		/// <summary>
		/// Resets the <see cref="RecoilValue{T}"/>  back to it's default value
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="atom">The atom to reset</param>
		void ResetValue<T>(Atom<T> atom);

		/// <summary>
		/// Sets the value of the <see cref="RecoilValue{T}"/> in the store and processes 
		/// all the update events on a background job
		/// </summary>
		/// <typeparam name="T">The values type</typeparam>
		/// <param name="atom">The atom to set</param>
		/// <param name="value">The value to set it too</param>
		void SetValue<T>(Atom<T> atom, T? value);

		/// <summary>
		/// Sets the value of the store and awaits for all the update events to be fired.
		/// </summary>
		/// <typeparam name="T">The values type</typeparam>
		/// <param name="recoilObject">The atom to set</param>
		/// <param name="value">The value to set it too</param>
		Task SetValueAsync<T>(Atom<T> atom, T? value);

		/// <summary>
		/// Creates a new <see cref="RecoilState"/> from the current store
		/// </summary>
		/// <typeparam name="T">The type of the state</typeparam>
		/// <param name="atom">The atom to use</param>
		/// <returns>The newly created state</returns>
		RecoilState<T> UseState<T>(Atom<T> atom);

		/// <summary>
		/// Creates a new <see cref="RecoilState"/> from the given store
		/// </summary>
		/// <typeparam name="T">The type of the state</typeparam>
		/// <param name="selector">The selector to use</param>
		/// <returns>The newly created satte</returns>
		RecoilState<T> UseState<T>(Selector<T> selector);
	}
}
