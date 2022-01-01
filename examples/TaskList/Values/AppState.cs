#pragma warning disable CA1416 // Validate platform compatibility
using RecoilNet;
using RecoilNet.Interfaces;
using System.Threading.Tasks;
using TaskList.Data;

namespace TaskList.Values
{
	public static class AppState
	{
		public static readonly Selector<SubmissionSummary> SubmissionSummary;

		static AppState()
		{
			SubmissionSummary = new Selector<SubmissionSummary>("Submission.Summary", builder: FormatSummary);

		}

		public static async Task<SubmissionSummary?> FormatSummary(IValueProvider get)
		{
			string? username = await get.GetAsync(UserState.FirstName);
			int changelist = await get.GetAsync(PerforceState.ChangelistNumber);

			return new SubmissionSummary()
			{
				UserName = username,
				ChangelistId = changelist
			};
		}
	}
}
