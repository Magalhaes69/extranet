

using Domain.Models;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Persistence.SAP_Helps
{
    public sealed class SapSessionPool
    {
        private readonly ConcurrentQueue<SapSessionSlot> _free = new();
        private readonly SemaphoreSlim _available;

        public SapSessionPool(IOptions<SapSettings> options)
        {
            var size = Math.Max(1, options.Value.SessionPoolSize);
            _available = new SemaphoreSlim(size, size);
            for (var i = 0; i < size; i++)
                _free.Enqueue(new SapSessionSlot());
        }

        public async Task<SapSessionSlot> RentAsync(CancellationToken ct)
        {
            await _available.WaitAsync(ct);
            _free.TryDequeue(out var slot);
            return slot!;
        }

        public void Return(SapSessionSlot slot)
        {
            _free.Enqueue(slot);
            _available.Release();
        }
    }
}
