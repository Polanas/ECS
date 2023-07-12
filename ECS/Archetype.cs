using System.Runtime.CompilerServices;

namespace ECS;

public sealed class ArchetypeEdge
{
    public WeakReference? Add;
    public WeakReference? Remove;
}

public sealed class Archetype
{
    public ulong[] Entities => _entities;
    public Array[] Storages => _table.Storages;
    public bool IsEmpty => Count == 0;
    public Table Table => _table;

    public int Count { get; internal set; }
    public readonly int id;
    public readonly SortedSet<ulong> components;
    public readonly ulong[] componentsArray;

    private readonly Archetypes _archetypes;
    private ulong[] _entities;
    private readonly Dictionary<ulong, ArchetypeEdge> _edges;
    private readonly Table _table;

    public Archetype(Table table, int id, Archetypes archetypes, SortedSet<ulong> components, int capacity)
    {
        _edges = new();
        _table = table;
        _archetypes = archetypes;
        _entities = new ulong[capacity];
        this.id = id;
        this.components = components;

        componentsArray = new ulong[components.Count];

        int index = 0;
        foreach (var component in components)
            componentsArray[index++] = component;
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetStorage<T>(out T[]? storage) where T : struct
    {
        return _table.TryGetStorage(out storage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetStorage<T>(ulong type, out T[]? storage) where T : struct
    {
        return _table.TryGetStorage(type, out storage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Array GetStorage(ulong type)
    {
        return _table.GetStorage(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>() where T : struct
    {
        return _table.GetStorage<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>(ulong type) where T : struct
    {
        return _table.GetStorage<T>(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(ulong entity, out int tableRow, bool addToTable = true)
    {
        tableRow = addToTable ? _table.Add(entity) : -1;

        EnsureCapacity(Count + 1);
        _entities[(ulong)Count] = entity;
        return Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int archetypeRow, int tableRow, bool removeFromTable = true)
    {
        if (removeFromTable)
            _table.Remove(tableRow);

        Count--;

        if (archetypeRow < Count)
        {
            _entities[archetypeRow] = _entities[Count];
            _archetypes.GetEntityRecord(_entities[archetypeRow]).archetypeRow = archetypeRow;
        }

        _entities[Count] = default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveAll()
    {
        Count = 0;
        Array.Clear(_entities);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArchetypeEdge GetTableEdge(ulong typeIndex)
    {
        if (_edges.TryGetValue(typeIndex, out var edge))
            return edge;

        edge = new();
        _edges.Add(typeIndex, edge);

        return edge;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void EnsureCapacity(int capacity)
    {
        if (capacity <= _entities.Length)
            return;

        var newCapacity = (capacity - 1) << 1;

        Array.Resize(ref _entities, newCapacity);
    }
}