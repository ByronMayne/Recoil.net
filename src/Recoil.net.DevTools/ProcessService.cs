using Recoil.net.DevTools.Data;
using Recoil.net.DevTools.State;
using RecoilNet;
using RecoilNet.State;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recoil.net.DevTools
{
	/// <summary>
	/// This polls the running processes in attempt to find all of the process that are running
	/// with the <see cref="Recoil"/> assembly.
	/// </summary>
	internal class ProcessService
	{
		private CancellationTokenSource? m_cancellationTokenSource;
		private readonly string m_recoilModuleName;
		private readonly IRecoilStore m_store;

		private RecoilState<IReadOnlyList<ProcessTarget>> m_processes;


		/// <summary>
		/// Gets the amount of time between polling attempts 
		/// </summary>
		public TimeSpan PollInterval { get; init; }


		public ProcessService(IRecoilStore recoilStore)
		{
			m_store = recoilStore;
			m_processes = new RecoilState<IReadOnlyList<ProcessTarget>>(ProcessState.Processes, m_store);

			PollInterval = TimeSpan.FromSeconds(5);
			m_recoilModuleName = $"Recoil.net.dll";
		}


		public void Start()
		{
			m_cancellationTokenSource?.Cancel();
			m_cancellationTokenSource = new CancellationTokenSource();
			Task.Run(() => RunAsync(m_cancellationTokenSource.Token));
		}

		/// <summary>
		/// Runs in the background and polls for updates the the services that we can attach too
		/// </summary>
		/// <param name="token">The toekn to cancel on</param>																					    
		private async void RunAsync(CancellationToken cancellationToken)
		{
			// Keep track of all assemblies we know we can't load 
			HashSet<string> skipProcessNames = new HashSet<string>()
			{
				"svchost",
				"idle",
			};

			while (!cancellationToken.IsCancellationRequested)
			{
				ConcurrentBag<ProcessTarget> processTargets = new ConcurrentBag<ProcessTarget>();

				Parallel.ForEach(Process.GetProcesses(), (process) =>
				{
					if(process.HandleCount == 0)
					{
						// Process that we don't 
						return;
					}

					if (skipProcessNames.Contains(process.ProcessName))
					{
						return;
					}

					try
					{
						foreach (ProcessModule module in process.Modules)
						{
							string? moduleName = module.ModuleName;

							if (m_recoilModuleName.Equals(moduleName))
							{
								processTargets.Add(new ProcessTarget(process.ProcessName, process.Id));
								break;
							}
						}
					}
					catch
					{
						skipProcessNames.Add(process.ProcessName);
					}
				});
				await ProcessState.Processes.SetValueAsync(m_store, processTargets.ToArray());
				await Task.Delay(PollInterval);
			}
		}
	}
}
