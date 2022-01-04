using RecoilNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoil.net.DevTools.State
{
	internal static class InspectorState
	{
		public static Atom<IReadOnlyList<string>> Pages { get; }
		public static Atom<string> ActivePage { get; }


		static InspectorState()
		{
			Pages = new Atom<IReadOnlyList<string>>("InspectorState.Pages");
			ActivePage = new Atom<string>("InspectorState.ActivePage", "/Pages/EventViewerPage");
		}

	}
}
