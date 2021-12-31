namespace RecoilNet
{
	/// <summary>
	/// THis class provides helper functions for working with RecoilNet libray.
	/// </summary>
	public static class Recoil
	{
		/// <summary>
		/// Creates a new Atom with no defalut value defined
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="key">The unique key for the atom</param>
		/// <returns>The created atom</returns>
		public static Atom<T> CreateAtom<T>(string key)
			=> new Atom<T>(key);

		/// <summary>
		/// Creates a new <see cref="Atom{T}"/> with a defined defalut value
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="key">The unique key for the atom</param>
		/// <param name="defaultValue">The default value of the atom</param>
		/// <returns>The created atom</returns>
		public static Atom<T> CreateAtom<T>(string key, T? defaultValue)
			=> new Atom<T>(key, defaultValue);

		/// <summary>
		/// Creates a new <see cref="Atom{T}"/> with a defined defalut value which is
		/// provided by another atom.
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="key">The unique key for the atom</param>
		/// <param name="defaultValue">The default value of the atom</param>
		/// <returns>The created atom</returns>
		public static Atom<T> CreateAtom<T>(string key, Atom<T> atom)
			=> new Atom<T>(key, atom);

		/// <summary>
		/// Creates a new <see cref="Atom{T}"/> with a defined defalut value that is provided
		/// by a <see cref="Selector{T}"/>
		/// </summary>
		/// <typeparam name="T">The value type</typeparam>
		/// <param name="key">The unique key for the atom</param>
		/// <param name="defaultValue">The default value of the atom</param>
		/// <returns>The created atom</returns>
		public static Atom<T> CreateAtom<T>(string key, Selector<T> defaultValue)
			=> new Atom<T>(key, defaultValue);

		/// <summary>
		/// Creates a new selector for the given type
		/// </summary>
		/// <typeparam name="T">The value type the selector produces</typeparam>
		/// <param name="key">The unique key for the selector</param>
		/// <param name="get">The function to build the selector</param>
		/// <returns>The created selector</returns>
		public static Selector<T> CreateSelector<T>(string key, Selector<T>.GetValueDelegate get)
			=> new Selector<T>(key, get);

	}
}