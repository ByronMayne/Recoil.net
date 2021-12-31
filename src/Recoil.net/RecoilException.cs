using System.Runtime.Serialization;

namespace Recoil.net
{
	/// <summary>
	/// Base exception thrown for all known exceptions in Recoil
	/// </summary>
	internal class RecoilException : Exception
	{
		public RecoilException()
		{
		}

		public RecoilException(string? message) : base(message)
		{
		}

		public RecoilException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected RecoilException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
