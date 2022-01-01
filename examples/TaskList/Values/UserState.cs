using RecoilNet;
using RecoilNet.Interfaces;
using System.Threading.Tasks;

namespace TaskList.Values
{
	public static class UserState
	{
		public static readonly Atom<string> FirstName;
		public static readonly Atom<string> LastName;
		public static readonly Selector<string> Username;
		public static readonly Selector<string> FullName;

		static UserState()
		{
			FirstName = Recoil.CreateAtom("User.FirstName", "John");
			LastName = Recoil.CreateAtom("User.LastName", "Smith");
			Username = Recoil.CreateSelector("User.UserName", GetUserName);
			FullName = new Selector<string>("User.FullName", GetFullName);
		}

		private static async Task<string?> GetUserName(IValueProvider provider)
		{
			string? firstName = await provider.GetAsync(FirstName);
			string? lastName = await provider.GetAsync(LastName);

			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
			{
				return "";
			}

			return $"{ char.ToLower(firstName[0])}{lastName.ToLower()}";
		}

		private static async Task<string?> GetFullName(IValueProvider provider)
		{
			string? firstName = await provider.GetAsync(FirstName);
			string? lastName = await provider.GetAsync(LastName);

			return $"{firstName} {lastName}";
		}
	}
}
