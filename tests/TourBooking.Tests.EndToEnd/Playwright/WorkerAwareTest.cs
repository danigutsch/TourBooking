using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace TourBooking.Tests.EndToEnd.Playwright;

[PublicAPI]
public class WorkerAwareTest : ExceptionCapturer
{
    private Worker _currentWorker = null!;

    public int WorkerIndex { get; internal set; }
    private static readonly ConcurrentStack<Worker> AllWorkers = new();

    public async Task<T> RegisterService<T>(string name, Func<Task<T>> factory) where T : class, IWorkerService
    {
        ArgumentNullException.ThrowIfNull(factory);

        if (!_currentWorker.Services.TryGetValue(name, out var service))
        {
            _currentWorker.Services[name] = await factory().ConfigureAwait(false);
        }

        return (service as T)!;
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        if (!AllWorkers.TryPop(out _currentWorker!))
        {
            _currentWorker = new Worker();
        }

        WorkerIndex = _currentWorker.WorkerIndex;
        if (PlaywrightSettingsProvider.ExpectTimeout.HasValue)
        {
            Assertions.SetDefaultExpectTimeout(PlaywrightSettingsProvider.ExpectTimeout.Value);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (TestOk)
        {
            foreach (var kv in _currentWorker.Services)
            {
                await kv.Value.ResetAsync().ConfigureAwait(false);
            }

            AllWorkers.Push(_currentWorker);
        }
        else
        {
            foreach (var kv in _currentWorker.Services)
            {
                await kv.Value.DisposeAsync().ConfigureAwait(false);
            }

            _currentWorker.Services.Clear();
        }

        await base.DisposeAsync().ConfigureAwait(false);
    }

    internal sealed class Worker
    {
        public Dictionary<string, IWorkerService> Services = [];
        public int WorkerIndex = Interlocked.Increment(ref _lastWorkedIndex);
        private static int _lastWorkedIndex;
    }
}