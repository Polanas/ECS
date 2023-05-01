using System.Runtime.CompilerServices;

namespace ECS;

public sealed class Table
{
    public Array[] Storages => _storages;
    public SortedSet<ulong> Components => _components;
    public int Count { get; private set; }

    private Array[] _storages;
    private readonly Dictionary<ulong, int> _indices;
    private SortedSet<ulong> _components;
    private Archetypes _archetypes;

    public Table(Archetypes archetypes, SortedSet<ulong> components, int defaultCapacity, int relationshipsCapacity)
    {
        _indices = new();
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
            bool isRelatonship = _archetypes.IsDataRelationship(typeId);
            var typeIndex = isRelatonship ? _archetypes.GetDataRelationshipType(typeId) : TypeData.TypesByIndices[typeId];
            _storages[index] = Array.CreateInstance(typeIndex, isRelatonship ? relationshipsCapacity : defaultCapacity);
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
    public bool TryGetStorage<T>(out T[]? storage) where T : struct
    {
        storage = null;
        var type = TypeData<T>.index;

        if (!_indices.TryGetValue(type, out var index))
            return false;

        storage =  Unsafe.As<T[]>(_storages[index]);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add()
    {
        EnsureCapacity(Count + 1);
        return Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int row)
    {
        Count--;

        if (Count < 0)
        {

        }

        if (row < Count)
        {
            foreach (var storage in _storages)
            {
                Array.Copy(storage, Count, storage, row, 1);
            }
        }
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

        table.Count = 0;
    }

    [Obsolete]
    public static void MoveEntities(Archetype oldArchetype, Archetype newArchetype)
    {
        var oldTable = oldArchetype.Table;
        var newTable = newArchetype.Table;

        newArchetype.EnsureCapacity(newArchetype.Count + oldArchetype.Count);
        newTable.EnsureCapacity(newTable.Count + oldTable.Count);

        foreach (var (type, oldIndex) in oldTable._indices)
        {
            if (!newArchetype.Table._indices.TryGetValue(type, out var newIndex))
                continue;

            var oldStorage = oldTable._storages[oldIndex];
            var newStorage = newTable._storages[newIndex];

            Array.Copy(oldStorage, 0, newStorage, newTable.Count, oldTable.Count);
        }

        newTable.Count += oldTable.Count;
        newArchetype.Count += oldArchetype.Count;

        oldTable.Count = 0;
        oldArchetype.RemoveAll();
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
    }
}