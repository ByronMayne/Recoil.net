using System.Diagnostics;

namespace RecoilNet.Effects
{
	/// <summary>
	/// Adds a log entry every time an atom's value changes
	/// </summary>
	public class LogChangePrimitiveEffect<T> : IPrimitiveEffect<T>
	{
		public void OnSet(T? newValue, T? oldValue, bool isReset)
		{
			Debug.WriteLine($"{oldValue} -> {newValue}");
		}
	}
}
