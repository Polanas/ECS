using System.Runtime.CompilerServices;

namespace ECS;

public sealed class Table
{
    public Array[] Storages => _storages;
    public SortedSet<ulong> Components => _components;
    public int Count { get; private set; }

    private readonly Array[] _storages;
    private readonly Dictionary<ulong, int> _indices;
    private readonly SortedSet<ulong> _components;
    private readonly Archetypes _archetypes;
    private ulong[] _entites = null!; 

    public Table(Archetypes archetypes, SortedSet<ulong> components, int defaultCapacity, int relationshipsCapacity)
    {
        _indices = new();
        _entites = new ulong[defaultCapacity];
        _archetypes = archetypes;

        _components = GetTypes(components);
        _storages = new Array[_components.Count];

        int i = 0;
        foreach (var type in _components)
        {
            _indices.Add(type, i++);
        }

        foreach (var (typeId, index) in _indices)
        {
            bool isDataRelatonship = _archetypes.IsDataRelationship(typeId);
            var typeIndex = isDataRelatonship ? _archetypes.GetDataRelationshipType(typeId) : TypeData.TypesByIndices[typeId];
            _storages[index] = Array.CreateInstance(typeIndex, isDataRelatonship ? relationshipsCapacity : defaultCapacity);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Array GetStorage(ulong type)
    {
        return _storages[_indices[type]];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>(ulong type) where T : struct
    {
        return Unsafe.As<T[]>(_storages[_indices[type]]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>() where T : struct
    {
        var type = TypeData<T>.index;
        return Unsafe.As<T[]>(_storages[_indices[type]]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasStorage(ulong type)
    {
        return _indices.ContainsKey(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetStorage<T>(ulong type, out T[]? storage) where T : struct
    {
        storage = null;

        if (!_indices.TryGetValue(type, out var index))
            return false;

        storage = Unsafe.As<T[]>(_storages[index]);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetStorage<T>(out T[]? storage) where T : struct
    {
        storage = null;
        var type = TypeData<T>.index;

        if (!_indices.TryGetValue(type, out var index))
            return false;

        storage = Unsafe.As<T[]>(_storages[index]);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(Entity entity)
    {
        EnsureCapacity(Count + 1);
        _entites[Count] = entity;
        return Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int row)
    {
        Count--;

        if (row >= Count)
            return;

        foreach (var storage in _storages)
        {
            Array.Copy(storage, Count, storage, row, 1);
        }

        _archetypes.GetEntityRecord(_entites[Count]).tableRow = row;
        _entites[row] = _entites[Count];
    }

    public static void RemoveEntities(Archetype archetype)
    {
        if (archetype.IsEmpty)
            return;

        var table = archetype.Table;

        foreach (var storage in table._storages)
        {
            Array.Clear(storage);
        }

        Array.Clear(table._entites);

        table.Count = 0;
    }

    public static int MoveEntity(ulong entity, int oldArchetypeRow, int oldTableRow, Archetype oldArchetype, Archetype newArchetype, out int tableRow)
    {
        var oldTable = oldArchetype.Table;
        var newTable = newArchetype.Table;
        var newRow = newArchetype.Add(entity, out tableRow);

        foreach (var (type, oldIndex) in oldTable._indices)
        {
            if (!newArchetype.Table._indices.TryGetValue(type, out var newIndex))
                continue;

            var oldStorage = oldTable._storages[oldIndex];
            var newStorage = newTable._storages[newIndex];

            Array.Copy(oldStorage, oldTableRow, newStorage, tableRow, 1);
        }

        oldArchetype.Remove(oldArchetypeRow, oldTableRow);

        return newRow;
    }

    private SortedSet<ulong> GetTypes(SortedSet<ulong> originalTypes)
    {
        bool typesAreDifferent = false;
        foreach (var type in originalTypes)
        {
            if (!_archetypes.IsDataComponent(type))
            {
                typesAreDifferent = true;
                break;
            }
        }

        if (!typesAreDifferent)
            return originalTypes;

        var newTypes = new SortedSet<ulong>();
        foreach (var type in originalTypes)
        {
            if (_archetypes.IsDataComponent(type))
                newTypes.Add(type);
        }

        return newTypes;
    }

    private void EnsureCapacity(int capacity)
    {
        if (_storages.Length > 0 && capacity <= _storages[0].Length)
            return;

        var newCapacity = (capacity - 1) << 1;

        for (int i = 0; i < _storages.Length; i++)
        {
            var elementType = _storages[i].GetType().GetElementType()!;
            var newStorage = Array.CreateInstance(elementType, newCapacity);
            Array.Copy(_storages[i], newStorage, capacity - 1);
            _storages[i] = newStorage;
        }

        Array.Resize(ref _entites, newCapacity);
    }
}