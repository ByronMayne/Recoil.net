using RecoilNet.State;
using RecoilNet.Utility;
using RecoilNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Recoil.net.Utility
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
			return root != null
				? root.Store
				: throw ErrorFactory.NoRecoilRootFound(element);
		}
	}
}
