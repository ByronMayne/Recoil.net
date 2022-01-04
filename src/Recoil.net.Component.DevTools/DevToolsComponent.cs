using Recoil.Components.DevTools.Payloads;
using RecoilNet;
using RecoilNet.Components;
using RecoilNet.State;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace Recoil.Components.DevTools
{
	/// <summary>
	/// Used to write a record of this process running so that it can be picked
	/// up by the development tools 
	/// </summary>
	internal class DevToolsComponent : IStoreComponent
	{
		private readonly CancellationTokenSource m_cancellationTokenSource;
		private readonly NamedPipeServerStream m_pipeServer;

		public string PipeName { get; }

		public DevToolsComponent()
		{
			PipeName = $"Recoil_DevTools_{Process.GetCurrentProcess().Id}";
			m_pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut);
			m_cancellationTokenSource = new CancellationTokenSource();
		}

		public void Initialize(IRecoilStore store)
		{
			Task.Run(() => ListenAsync());
		}

		public void OnStateAdded<T>(RecoilState<T> state, IRecoilStore recoilStore)
		{
			SendObject(new StateAddedPayload(state.RecoilValue.Key, recoilStore.Id));
		}

		public void OnStateRemoved<T>(RecoilState<T> state, IRecoilStore recoilStore)
		{
			SendObject(new StateRemovedPayload(state.RecoilValue.Key, recoilStore.Id));
		}

		public void OnValueChanged<T>(RecoilStore recoilStore, Atom<T> changedAtom, T? value, HashSet<RecoilValue> dependents)
		{
			StoreValueChangedPayload payload = new StoreValueChangedPayload()
			{
				StoreId = recoilStore.Id,
				Key = changedAtom.Key,
				DependentKeys = dependents.Select(dependent => dependent.Key).ToArray(),
				JsonValue = value == null ? null : JsonSerializer.Serialize(value),
				HasValue = value != null,
				ValueTypeName = typeof(T).Name,
			};

			SendObject(payload);
		}


		private async void ListenAsync()
		{
			while (!m_cancellationTokenSource.IsCancellationRequested)
			{
				await m_pipeServer.WaitForConnectionAsync();

				while (m_pipeServer.IsConnected)
				{
					byte[] buffer = new byte[2048];

					//int byteCount = await m_pipeServer.ReadAsync(buffer, 0, buffer.Length);
					//string text = Encoding.UTF8.GetString(buffer, 0, byteCount);
					//OnTextReceived(text);
					await Task.Delay(TimeSpan.FromSeconds(1));
				}

			}
		}

		/// <summary>
		/// Serializes an object and then sends the value as a string
		/// over the named pipe.
		/// </summary>
		private void SendObject<T>(T data)
		{
			if (!m_pipeServer.IsConnected)
			{
				return;
			}

			string json = JsonSerializer.Serialize<T>(data);
			byte[] buffer = Encoding.UTF8.GetBytes(json);
			m_pipeServer.Write(buffer, 0, buffer.Length);
		}

		public void Dispose()
		{
			m_cancellationTokenSource.Cancel();
		}
	}
}
