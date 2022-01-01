using RecoilNet;
using RecoilNet.State;
using TaskList.Values;

namespace TaskList.Pages.Tasklist
{
	public class TaskListViewModel
	{
		/// <summary>
		/// Gets the first name of the user 
		/// </summary>
		public RecoilState<string> FirstName { get; }

		/// <summary>
		/// Gets the last name of the user 
		/// </summary>
		public RecoilState<string> LastName { get; }

		/// <summary>
		/// Gets the full name of the user
		/// </summary>
		public RecoilState<string> FullName { get; }

		/// <summary>
		/// Gets the summary of the changelist as written by the user 
		/// </summary>
		public RecoilState<string> ChangelistSummary { get; }

		/// <summary>
		/// Gets the description of the changelist 
		/// </summary>
		public RecoilState<string> ChangelistDescription { get; }

		/// <summary>
		/// Gets the number of the changelist that we are using 
		/// </summary>
		public RecoilState<int> ChangelistNumber { get; }


		public TaskListViewModel(IRecoilStore store)
		{
			FirstName = store.UseState(UserState.FirstName);
			LastName = store.UseState(UserState.LastName);
			FullName = store.UseState(UserState.FullName);
			ChangelistDescription = store.UseState(PerforceState.ChangelistDescription);
			ChangelistSummary = store.UseState(PerforceState.ChangelistUserSummary);
			ChangelistNumber = store.UseState(PerforceState.ChangelistNumber);
		}
	}
}
