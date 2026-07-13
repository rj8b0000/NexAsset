using System;
using System.Threading;

namespace NexAsset.Web.Infrastructure.Loading
{
    /// <summary>
    /// Default <see cref="IGlobalLoadingService"/>: a reference-counted in-flight-operation
    /// tracker. Not wired into any component yet — a future phase can inject it into a shell
    /// layout (e.g. a thin top-of-page progress bar) once real API calls exist to track.
    /// </summary>
    public class GlobalLoadingService : IGlobalLoadingService
    {
        private int _activeOperations;

        public bool IsLoading => Volatile.Read(ref _activeOperations) > 0;

        public event Action? OnChange;

        public IDisposable BeginOperation()
        {
            Interlocked.Increment(ref _activeOperations);
            OnChange?.Invoke();
            return new OperationScope(this);
        }

        private void EndOperation()
        {
            Interlocked.Decrement(ref _activeOperations);
            OnChange?.Invoke();
        }

        private sealed class OperationScope : IDisposable
        {
            private readonly GlobalLoadingService _owner;
            private bool _disposed;

            public OperationScope(GlobalLoadingService owner)
            {
                _owner = owner;
            }

            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;
                _owner.EndOperation();
            }
        }
    }
}
