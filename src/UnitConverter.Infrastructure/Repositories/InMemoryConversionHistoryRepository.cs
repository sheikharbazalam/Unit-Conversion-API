using System.Collections.Concurrent;
using UnitConverter.Application.Interfaces;
using UnitConverter.Domain.Entities;

namespace UnitConverter.Infrastructure.Repositories;

public sealed class InMemoryConversionHistoryRepository : IConversionHistoryRepository
{
    private const int Capacity = 500;
    private readonly ConcurrentQueue<ConversionRecord> _queue = new();

    public void Add(ConversionRecord record)
    {
        _queue.Enqueue(record);
        while (_queue.Count > Capacity)
            _queue.TryDequeue(out _);
    }

    public IReadOnlyList<ConversionRecord> GetRecent(int limit = 20) =>
        _queue.Reverse().Take(limit).ToList();
}
