using RecoilNet.Diagnostics;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RecoilNet
{
    /// <summary>
    /// Base class that contains logic shared amoung recoil types
    /// </summary>
    [DebuggerDisplay("{RenderDebug()}")]
	public abstract class Primitive : IEqualityComparer<Primitive>
	{
		public class EqualityComparer : IEqualityComparer<Primitive>
		{
			public bool Equals(Primitive? x, Primitive? y)
				=> ReferenceEquals(x, y);

			public int GetHashCode([DisallowNull] Primitive obj)
				=> obj.Key.GetHashCode();
		}

		protected readonly HashSet<Primitive> m_dependents;

		/// <summary>
		/// Gets the unique string used to identify the atom internally. 
		/// This string should be unique with respect to other atoms and selectors 
		/// in the entire application.
		/// </summary>
		public Key Key { get; }

        /// <summary>
        /// Gets the information about the creator of this recoil primitive.
        /// </summary>
        public CallerInfo CreatorInfo { get; }

		public Primitive DefaultValue { get; }

		/// <summary>
		/// Gets all the nodes that depend on this one for their value
		/// </summary>
		public IReadOnlyCollection<Primitive> Dependents
			=> m_dependents;

		/// <summary>
		/// Initializes a new instance of a recoil object.
		/// </summary>
		/// <param name="key">A unique key for the given object</param>
		/// <param name="creatorInfo">Contains information about who created this object, used for debugging</param>
		protected Primitive(Key key, CallerInfo creatorInfo)
		{
			Gaurd.NotNull(key);
			Key = key;
			CreatorInfo = creatorInfo;
            m_dependents = new HashSet<Primitive>();
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
		internal void AddDependent(Primitive primitive)
		{
			m_dependents.Add(primitive);
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
				case Primitive recoilObject:
					return ReferenceEquals(recoilObject, obj);
			}
			return false;
		}

		public bool Equals(Primitive? x, Primitive? y)
			=> object.ReferenceEquals(x, y);

		public int GetHashCode([DisallowNull] Primitive obj)
			=> obj.GetHashCode();

		/// <summary>
		/// Used to render the tooltip for debugger
		/// </summary>
		internal abstract string RenderDebug();
	}
}
