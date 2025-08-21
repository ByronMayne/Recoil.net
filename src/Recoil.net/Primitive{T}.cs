using RecoilNet.Diagnostics;

namespace RecoilNet
{
	/// <summary>
	/// Base class that contains logic shared among recoil types
	/// </summary>
	public abstract class Primitive<T> : Primitive
	{
		/// <summary>
		/// Gets if this object has the ability to be set directly 
		/// </summary>
		public bool IsMutable { get; }

		/// <inheritdoc cref="Primitive.Primitive(string)"/>
		public Primitive(Key key, CallerInfo creatorInfo, bool isMutable) : base(key, creatorInfo)
		{
			IsMutable = isMutable;
        }
	}
}
