using RecoilNet.Internal;
using RecoilNet.State;
using RecoilNet.Utility;
using System.Diagnostics;

namespace RecoilNet
{
	/// <summary>
	/// Base class that contains logic shared among recoil types
	/// </summary>
	public abstract class RecoilValue<T> : RecoilValue
	{
		/// <summary>
		/// Gets if this object has the ability to be set directly 
		/// </summary>
		public bool IsMutable { get; }

		/// <inheritdoc cref="RecoilValue.RecoilValue(string)"/>
		public RecoilValue(string key, bool isMutable) : base(key)
		{
			IsMutable = isMutable;
		}

		/// <summary>
		/// Gets the value of a <see cref="RecoilValue"/> from the instance of the store. 
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="recoilStore">The store instance to attempt to fetch it from</param>
		/// <returns>The value</returns>
		public abstract Task<T?> GetValueAsync(IRecoilStore? recoilStore);

		/// <summary>
		/// Sets this objects value to the given store if it has a value
		/// </summary>
		/// <param name="recoilStore">The store to assign the value</param>
		/// <param name="value">The value to set</param>
		public virtual async Task SetValueAsync(IRecoilStore? recoilStore, T? value)
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
	}
}
