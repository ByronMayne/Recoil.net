using RecoilNet;
using RecoilNet.State.Instructions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;

namespace RecoilNet.State
{
    public class RecoilStore : IRecoilStore
    {
        private readonly static IReadOnlyList<RecoilStore> s_stores;

        private readonly CancellationTokenSource m_cancellationTokenSource;
        private readonly ConcurrentBag<RecoilState> m_states;
        private readonly ConcurrentDictionary<Primitive, Task> m_pendingTasks = new();
        private readonly ConcurrentDictionary<Primitive, object?> m_values = new();
        private readonly ConcurrentQueue<Instruction> m_instructionsQueue;
        private readonly SemaphoreSlim m_instructionsSemaphore;

        public int Id { get; }


        static RecoilStore()
        {
            s_stores = new List<RecoilStore>();
        }

        public RecoilStore()
        {
            m_states = new ConcurrentBag<RecoilState>();
            m_pendingTasks = new ConcurrentDictionary<Primitive, Task>();
            m_values = new ConcurrentDictionary<Primitive, object?>();
            m_instructionsQueue = new ConcurrentQueue<Instruction>();
            m_instructionsSemaphore = new SemaphoreSlim(0);
            m_cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => ProcessInstructionsAsync(m_cancellationTokenSource.Token), 
                TaskCreationOptions.LongRunning);
        }

        /// <inheritdoc cref="GetAsync(Primitive, CancellationToken)"/>
        public async Task<object?> GetAsync(Primitive primitive, CancellationToken cancellationToken = default)
        {
            // If a set is in progress, wait for it to complete
            if (m_pendingTasks.TryGetValue(primitive, out var pendingTask))
            {
                await pendingTask.ConfigureAwait(false);
            }

            // Try to get the value if available
            if (m_values.TryGetValue(primitive, out object? value))
            {
                return value;
            }

            // Otherwise, fetch from primitive
            return await GetAsync(primitive.DefaultValue);
        }


        public void Set(Primitive primitive, object? value)
        {
            SetInstruction instruction = new SetInstruction(primitive, value);
            m_instructionsQueue.Enqueue(instruction);
        }

        private async Task ProcessInstructionsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await m_instructionsSemaphore.WaitAsync(cancellationToken);

                if(m_instructionsQueue.TryDequeue(out Instruction? instruction))
                {
                    switch (instruction)
                    {
                        case SetInstruction set:
                            m_values[set.Primitive] = set.Value;
                            break;
                        case ResetInstruction reset:
                            m_values.TryRemove(reset.Primitive, out _);
                            break;
                    }

                    await NotifyDependentsAsync(instruction.Primitive, cancellationToken);
                }
            }
        }

        private async Task NotifyDependentsAsync(Primitive primitive, CancellationToken cancellationToken)
        {
            HashSet<Primitive> dependents = new HashSet<Primitive>();
            GetDependents(primitive, dependents);

            foreach (RecoilState state in m_states)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (state.Primitive == primitive)
                {
                    var stateValue = await GetAsync(state.Primitive);
                    await state.ValueChangedAsync(this, stateValue);
                }
                else if (dependents.Contains(state.Primitive))
                {
                    await state.DependentChangedAsync(this, state.Primitive);
                }
            }
        }

        private static void GetDependents(Primitive current, HashSet<Primitive> dependents)
        {
            if (current.Dependents.Count > 0)
            {
                foreach (Primitive dependent in current.Dependents)
                {
                    dependents.Add(dependent);

                    GetDependents(dependent, dependents);
                }
            }
        }

        public void Dispose()
        {
            m_cancellationTokenSource.Cancel();
        }
    }
}
