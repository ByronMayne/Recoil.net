using System.Windows;
using System.Windows.Media;

namespace RecoilNet
{
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Walks the elements ancestors and attempts to find a value in the tree and returns it back
		/// </summary>
		public static T? FindTypeInAncestors<T>(this DependencyObject? instance)
		{
			DependencyObject? current = instance;

			while (current != null)
			{
				if (current is T asT)
				{
					return asT;
				}

				current = VisualTreeHelper.GetParent(current);
			}

			return default;
		}
	}
}