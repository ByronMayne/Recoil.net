using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RecoilNet
{
	/// <summary>
	/// Base class that contains logic shared amoung recoile types
	/// </summary>
	[DebuggerDisplay("{RenderDebug()}")]
	public abstract class RecoilValue : IEqualityComparer<RecoilValue>
	{
		public class EqualityComparer : IEqualityComparer<RecoilValue>
		{
			public bool Equals(RecoilValue? x, RecoilValue? y)
				=> ReferenceEquals(x, y);

			public int GetHashCode([DisallowNull] RecoilValue obj)
				=> obj.Key.GetHashCode();
		}

		protected readonly HashSet<RecoilValue> m_dependents;

		/// <summary>
		/// Gets the unique string used to identify the atom internally. 
		/// This string should be unique with respect to other atoms and selectors 
		/// in the entire application.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Gets all the nodes that depend on this one for their value
		/// </summary>
		public IReadOnlyCollection<RecoilValue> Dependents
			=> m_dependents;

		/// <summary>
		/// Initializes a new instance of a recoil object.
		/// </summary>
		/// <param name="key">A unique key for the given object</param>
		protected RecoilValue(string key)
		{
			ArgumentNullException.ThrowIfNull(key);
			Key = key;
			m_dependents = new HashSet<RecoilValue>();
		}

		/// <summary>
		/// Gets the unique hash code for the object
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}

		/// <summary>
		/// Adds a new recoil object that depends on this one
		/// </summary>
		internal void AddDependent(RecoilValue recoilObject)
		{
			m_dependents.Add(recoilObject);
		}

		/// <summary>
		/// Checks if an object equals another 
		/// </summary>
		public override bool Equals(object? obj)
		{
			switch (obj)
			{
				case string asString:
					return string.Equals(Key, asString, StringComparison.Ordinal);
				case RecoilValue recoilObject:
					return ReferenceEquals(recoilObject, obj);
			}
			return false;
		}

		public bool Equals(RecoilValue? x, RecoilValue? y)
			=> object.ReferenceEquals(x, y);

		public int GetHashCode([DisallowNull] RecoilValue obj)
			=> obj.GetHashCode();

		/// <summary>
		/// Used to render the tooltip for debugger
		/// </summary>
		internal abstract string RenderDebug();
	}
}
