using RecoilNet.State;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

			// We allow windows and pages to use their direct child as stores 
			switch (element)
			{
				case ContentControl contentControl:
					{
						if (contentControl.Content is RecoilRoot recoilRoot)
						{
							return recoilRoot.Store;
						}
					}
					break;
				case Page asPage:
					{
						if (asPage.Content is RecoilRoot recoilRoot)
						{
							return recoilRoot.Store;
						}
					}
					break;
			}

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
