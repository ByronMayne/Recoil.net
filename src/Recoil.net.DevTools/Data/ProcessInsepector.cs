using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Recoil.net.DevTools.Data
{
	public class ProcessInsepector : IDisposable
	{
		private readonly CancellationTokenSource m_cancellationTokenSource;

		public string Name { get; }

		public int ProcessId { get; }

		public string PipeName { get; }


		private NamedPipeClientStream m_clientStream;


		public ProcessInsepector(int processId)
		{
			Process process = Process.GetProcessById(processId);
			ProcessId = process.Id;
			Name = process.ProcessName;
			PipeName = $"Recoil_DevTools_{ProcessId}";
			m_clientStream = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
			m_cancellationTokenSource = new CancellationTokenSource();
			Task.Run(() => ListenAsync(m_cancellationTokenSource.Token));
		}

		private void OnTextReceived(string message)
		{
			Debug.WriteLine(message);
		}	  

		private async void ListenAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				m_clientStream.Connect();


				while (m_clientStream.IsConnected)
				{
					byte[] buffer = new byte[2048];
					int byteCount = await m_clientStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
					string text = Encoding.UTF8.GetString(buffer, 0, byteCount);
					OnTextReceived(text);
				}
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		public void Dispose()
		{
			m_cancellationTokenSource.Cancel();
		}
	}
}
