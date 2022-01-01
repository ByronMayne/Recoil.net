using RecoilNet;
using RecoilNet.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskList.Data;

namespace TaskList.Values
{
	public static class PerforceState
	{
		public static readonly Atom<int> ChangelistNumber;
		public static readonly Atom<string> ChangelistUserSummary;
		public static readonly Selector<string> ChangelistDescription;

		public static readonly Atom<IReadOnlyList<Changelist>> Changelists;

		static PerforceState()
		{
			Changelists = Recoil.CreateAtom<IReadOnlyList<Changelist>>("Perforce.Changelists");
			ChangelistNumber = Recoil.CreateAtom("Perforce.ChangelistNumber", 0);
			ChangelistUserSummary = Recoil.CreateAtom("Perforce.ChangelistUserSummary", "Created by perforce");
			ChangelistDescription = new Selector<string>("Changelist.Description", GetChangelistDescription);


		}


		private static async Task<string?> GetChangelistDescription(IValueProvider get)
		{
			return $"Changelist by {await get.GetAsync(UserState.Username)}"
					+ $"\n with the cl {await get.GetAsync(ChangelistNumber)} "
					+ $"\n{await get.GetAsync(ChangelistUserSummary)}";

		}
	}
}
