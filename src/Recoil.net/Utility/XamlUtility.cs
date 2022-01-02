using RecoilNet.State;
using System.ComponentModel;
using System.Windows;

namespace RecoilNet.Utility
{
	/// <summary>
	/// Contains utility functions for work with XAML
	/// </summary>
	public static class XamlUtility
	{
		/// <summary>
		/// Walks the hiarchy and attempts to find the recoil store 
		/// </summary>
		public static IRecoilStore GetRecoilStore(FrameworkElement element)
		{
			ArgumentNullException.ThrowIfNull(element);

			RecoilRoot? root = element.FindTypeInAncestors<RecoilRoot>();

			if (root == null)
			{
				// During design mode we don't want to throw exceptions
				return DesignerProperties.GetIsInDesignMode(element)
					? new RecoilStore()
					: throw ErrorFactory.NoRecoilRootFound(element);
			}

			return root.Store;
		}
	}
}
