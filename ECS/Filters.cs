﻿using System.Runtime.CompilerServices;

namespace ECS;

public class Filter
{
    public Mask Mask => _mask;
    public Archetypes Archetypes => archetypes;
    public int Count
    {
        get
        {
            int count = 0;

            for (int i = 0; i < archetypesList.Count; i++)
            {
                count += archetypesList[i].Count;
            }

            return count;
        }
    }
    public bool HasEntities
    {
        get
        {
            for (int i = 0; i < archetypesList.Count; i++)
            {
                if (archetypesList[i].Count > 0)
                    return true;
            }

            return false;
        }
    }

    public readonly List<Archetype> archetypesList;

    protected readonly Archetypes archetypes;

    private readonly Mask _mask;

    internal PackedBoolInt optionalFlags;
    internal IterationMode[] iterationModes = null!;

    public Filter(
        Archetypes archetypes,
        Mask mask,
        List<Archetype> archetypesList,
        PackedBoolInt optionalFlags)
    {
        this.optionalFlags = optionalFlags;
        this.archetypesList = archetypesList;
        this.archetypes = archetypes;
        _mask = mask;
    }

    public Entity GetFirst()
    {
        if (archetypesList.Count == 0 || !TryGetFirstEntity(out var entity))
#if DEBUG
            throw new Exception("Cannot get first _entity as no entites satisfy the filter.");
#else
            return new(0, archetypes.World);
#endif

        return new(entity, archetypes.World);
    }

    public void AddArchetype(Archetype archetype)
    {
        archetypesList.Add(archetype);
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(archetypes, archetypesList);
    }

    private bool TryGetFirstEntity(out ulong entity)
    {
        entity = 0;

        foreach (var archetype in archetypesList)
        {
            if (archetype.Count == 0)
                continue;

            entity = archetype.Entities[0];
            return true;
        }

        return false;
    }
}

public struct FilterData
{
    internal Archetypes archetypes;
    internal Mask mask;
    internal List<Archetype> archetypesList;
    internal List<ulong> terms;
    internal PackedBoolInt optionalFlags;
    internal IterationMode[] iterationModes;

    public FilterData(
        Archetypes archetypes,
        Mask mask,
        List<Archetype> archetypesList,
        List<ulong> terms,
        PackedBoolInt optionalFlags,
        IterationMode[] iterationModes)
    {
        this.archetypes = archetypes;
        this.mask = mask;
        this.archetypesList = archetypesList;
        this.terms = terms;
        this.optionalFlags = optionalFlags;
        this.iterationModes = iterationModes;
    }
}

public class Filter<C> : Filter
    where C : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
        filterData.archetypes,
        filterData.mask,
        filterData.archetypesList,
        filterData.optionalFlags)
    {
        iterationModes = filterData.iterationModes;
        terms = filterData.terms.ToArray();
    }

    public new Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2> : Filter
    where C1 : struct
    where C2 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
         filterData.archetypes,
         filterData.mask,
         filterData.archetypesList,
         filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
        filterData.archetypes,
        filterData.mask,
        filterData.archetypesList,
        filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3, C4> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
        filterData.archetypes,
        filterData.mask,
        filterData.archetypesList,
        filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3, C4, C5> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
        filterData.archetypes,
        filterData.mask,
        filterData.archetypesList,
        filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3, C4, C5, C6> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
          filterData.archetypes,
          filterData.mask,
          filterData.archetypesList,
          filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3, C4, C5, C6, C7> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
         filterData.archetypes,
         filterData.mask,
         filterData.archetypesList,
         filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(archetypes, archetypesList, terms, this);
    }
}

public class Filter<C1, C2, C3, C4, C5, C6, C7, C8> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    internal readonly ulong[] terms;

    public Filter(FilterData filterData) : base(
        filterData.archetypes,
        filterData.mask,
        filterData.archetypesList,
        filterData.optionalFlags)
    {
        terms = filterData.terms.ToArray();
        iterationModes = filterData.iterationModes;
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, archetypesList, terms, this);
    }
}

internal struct PairData
{
    public bool isPair;
    public int size;

    public PairData(bool isPair, int size)
    {
        this.isPair = isPair;
        this.size = size;
    }
}

public struct Enumerator : IDisposable
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList)
    {
        _archetypesList = archetypesList;
        _archetypes = archetypes;
        _entityIndex = -1;
        archetypes.Lock();
        _world = archetypes.World;

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        _entityStorage = _archetypesList[_archetypeIndex].Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        if (++_entityIndex < _archetypesList[_archetypeIndex].Count)
            return true;

        _entityIndex = 0;
        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry Current
    {
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
        };
    }
}

public struct EnumeratorSingle<C> : IDisposable
    where C : struct
{
    private readonly Archetype? _archetype;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C[]? _storage = null!;

    public EnumeratorSingle(Archetypes archetypes, Archetype? archetype)
    {
        _archetype = archetype;
        _archetypes = archetypes;
        _entityIndex = -1;
        archetypes.Lock();
        _world = archetypes.World;

        UpdateStorages();
    }

    public void UpdateStorages()
    {
        if (_archetype == null)
            return;

        _archetype.TryGetStorage(out _storage);
        _entityStorage = _archetype.Entities;
    }

    public bool MoveNext()
    {
        if (_archetype != null && ++_entityIndex < _archetype.Count)
            return true;

        return false;
    }

    public void Reset()
    {
        _entityIndex = -1;

        UpdateStorages();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item = ref _storage is null ? ref Unsafe.As<C[]>(_entityStorage)[0] : ref _storage[_entityIndex]
        };
    }
}

public struct Enumerator<C> : IDisposable
    where C : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C[]? _storage;

    public Enumerator(
        Archetypes archetypes,
        List<Archetype> archetypesList,
        ulong[] terms,
        Filter<C> filter)
    {
        _iterationModes = filter.iterationModes;
        _archetypesList = archetypesList;
        _archetypes = archetypes;
        _terms = terms;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        _currentTerms.Add(ListPool<ulong>.Get());
        UpdateStorages();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage = TrySetStorageWithTems(archetype, 0, ref _storage!);
        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorages()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        _entityStorage = archetype.Entities;
        if (archetype.Count == 0)
            return;

        TrySetStorageAndOptional(archetype, 0, ref _storage);
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;

        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorages();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorages();
    }

    public void Dispose()
    {
        _archetypes.Unlock();

        foreach (var list in _currentTerms)
        {
            ListPool<ulong>.Add(list);
        }
        ListPool<List<ulong>>.Add(_currentTerms);
    }

    public unsafe Entry<C> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item = ref _storage == null ? ref Unsafe.AsRef<C>((void*)null) : ref _storage[record.tableRow]
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }
}

public struct Enumerator<C1, C2> : IDisposable
    where C1 : struct
    where C2 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private readonly ECSWorld _world;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1 = null!;
    private C2[]? _storage2 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _iterationModes = filter.iterationModes;
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        archetypes.Lock();
        _world = archetypes.World;

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 2; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        _entityStorage = archetype.Entities;
        if (archetype.Count == 0)
            return;

        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public unsafe Entry<C1, C2> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref Unsafe.AsRef<C1>((void*)null) : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref Unsafe.AsRef<C2>((void*)null) : ref _storage2[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private readonly ECSWorld _world;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 3; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage1!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        _entityStorage = archetype.Entities;
        if (archetype.Count == 0)
            return;

        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3, C4> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;
    private C4[]? _storage4;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 4; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!) &
            TrySetStorageWithTems(archetype, 3, ref _storage4!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return;
        _entityStorage = _archetypesList[_archetypeIndex].Entities;

        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
        TrySetStorageAndOptional(archetype, 3, ref _storage4);
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3, C4> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
                item4 = ref _storage4 == null ? ref ArraySingle<C4>.value[0] : ref _storage4[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3, C4, C5> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;
    private C4[]? _storage4;
    private C5[]? _storage5;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 5; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!) &
            TrySetStorageWithTems(archetype, 3, ref _storage4!) &
            TrySetStorageWithTems(archetype, 4, ref _storage5!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return;
        _entityStorage = _archetypesList[_archetypeIndex].Entities;

        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
        TrySetStorageAndOptional(archetype, 3, ref _storage4);
        TrySetStorageAndOptional(archetype, 4, ref _storage5);
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3, C4, C5> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
                item4 = ref _storage4 == null ? ref ArraySingle<C4>.value[0] : ref _storage4[record.tableRow],
                item5 = ref _storage5 == null ? ref ArraySingle<C5>.value[0] : ref _storage5[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;
    private C4[]? _storage4;
    private C5[]? _storage5;
    private C6[]? _storage6;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 6; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!) &
            TrySetStorageWithTems(archetype, 3, ref _storage4!) &
            TrySetStorageWithTems(archetype, 4, ref _storage5!) &
            TrySetStorageWithTems(archetype, 5, ref _storage6!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
        TrySetStorageAndOptional(archetype, 3, ref _storage4);
        TrySetStorageAndOptional(archetype, 4, ref _storage5);
        TrySetStorageAndOptional(archetype, 5, ref _storage6);
        _entityStorage = _archetypesList[_archetypeIndex].Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3, C4, C5, C6> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
                item4 = ref _storage4 == null ? ref ArraySingle<C4>.value[0] : ref _storage4[record.tableRow],
                item5 = ref _storage5 == null ? ref ArraySingle<C5>.value[0] : ref _storage5[record.tableRow],
                item6 = ref _storage6 == null ? ref ArraySingle<C6>.value[0] : ref _storage6[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6, C7> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;
    private C4[]? _storage4;
    private C5[]? _storage5;
    private C6[]? _storage6;
    private C7[]? _storage7;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _archetypes = archetypes;
        _terms = terms;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 7; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }

    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!) &
            TrySetStorageWithTems(archetype, 3, ref _storage4!) &
            TrySetStorageWithTems(archetype, 4, ref _storage5!) &
            TrySetStorageWithTems(archetype, 5, ref _storage6!) &
            TrySetStorageWithTems(archetype, 6, ref _storage7!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
        TrySetStorageAndOptional(archetype, 3, ref _storage4);
        TrySetStorageAndOptional(archetype, 4, ref _storage5);
        TrySetStorageAndOptional(archetype, 5, ref _storage6);
        TrySetStorageAndOptional(archetype, 6, ref _storage7);
        _entityStorage = _archetypesList[_archetypeIndex].Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3, C4, C5, C6, C7> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
                item4 = ref _storage4 == null ? ref ArraySingle<C4>.value[0] : ref _storage4[record.tableRow],
                item5 = ref _storage5 == null ? ref ArraySingle<C5>.value[0] : ref _storage5[record.tableRow],
                item6 = ref _storage6 == null ? ref ArraySingle<C6>.value[0] : ref _storage6[record.tableRow],
                item7 = ref _storage7 == null ? ref ArraySingle<C7>.value[0] : ref _storage7[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ECSWorld _world;
    private readonly ulong[] _terms;
    private readonly IterationMode[] _iterationModes = null!;
    private readonly List<List<ulong>> _currentTerms;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[]? _storage1;
    private C2[]? _storage2;
    private C3[]? _storage3;
    private C4[]? _storage4;
    private C5[]? _storage5;
    private C6[]? _storage6;
    private C7[]? _storage7;
    private C8[]? _storage8;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms, Filter filter)
    {
        _archetypesList = archetypesList;
        _iterationModes = filter.iterationModes;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        _currentTerms = ListPool<List<ulong>>.Get();
        for (int i = 0; i < 7; i++)
            _currentTerms.Add(ListPool<ulong>.Get());

        UpdateStorage();
    }


    public bool UpdateStoragesWithTerms()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];
        if (archetype.Count == 0)
            return false;

        bool setStorage =
            TrySetStorageWithTems(archetype, 0, ref _storage1!) &
            TrySetStorageWithTems(archetype, 1, ref _storage2!) &
            TrySetStorageWithTems(archetype, 2, ref _storage3!) &
            TrySetStorageWithTems(archetype, 3, ref _storage4!) &
            TrySetStorageWithTems(archetype, 4, ref _storage5!) &
            TrySetStorageWithTems(archetype, 5, ref _storage6!) &
            TrySetStorageWithTems(archetype, 6, ref _storage7!) &
            TrySetStorageWithTems(archetype, 7, ref _storage8!);

        return setStorage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetStorageWithTems<T>(
        Archetype archetype,
        int componentIndex,
        ref T[] storage) where T : struct
    {
        var currentTerms = _currentTerms[componentIndex];

        if (_iterationModes[componentIndex] == IterationMode.Single)
            return false;
        if (currentTerms.Count == 0)
            return false;

        storage = archetype.GetStorage<T>(currentTerms[^1]);
        currentTerms.RemoveAt(currentTerms.Count - 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<ulong> SetCorrectTerms(ulong term, Archetype archetype, int componentIndex)
    {
        List<ulong> terms = _currentTerms[componentIndex];

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
        {
            _archetypes.FindRelationships(archetype, term, relationIsWildcard, terms);
        }
        else
        {
            terms.Add(term);
        }

        return terms;
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        TrySetStorageAndOptional(archetype, 0, ref _storage1);
        TrySetStorageAndOptional(archetype, 1, ref _storage2);
        TrySetStorageAndOptional(archetype, 2, ref _storage3);
        TrySetStorageAndOptional(archetype, 3, ref _storage4);
        TrySetStorageAndOptional(archetype, 4, ref _storage5);
        TrySetStorageAndOptional(archetype, 5, ref _storage6);
        TrySetStorageAndOptional(archetype, 6, ref _storage7);
        TrySetStorageAndOptional(archetype, 7, ref _storage8);
        _entityStorage = _archetypesList[_archetypeIndex].Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
            return true;

        _entityIndex = 0;
        if (UpdateStoragesWithTerms())
            return true;

        _archetypeIndex++;

        while (_archetypeIndex < _archetypesList.Count && _archetypesList[_archetypeIndex].IsEmpty)
        {
            _archetypeIndex++;
        }

        UpdateStorage();

        return _archetypeIndex < _archetypesList.Count && _entityIndex < _archetypesList[_archetypeIndex].Count;
    }

    public void Reset()
    {
        _archetypeIndex = 0;
        _entityIndex = -1;

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C1, C2, C3, C4, C5, C6, C7, C8> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var entity = _entityStorage[_entityIndex];
            ref var record = ref _archetypes.GetEntityRecord(entity);

            return new()
            {
                entity = new Entity(entity, _world),
                item1 = ref _storage1 == null ? ref ArraySingle<C1>.value[0] : ref _storage1[record.tableRow],
                item2 = ref _storage2 == null ? ref ArraySingle<C2>.value[0] : ref _storage2[record.tableRow],
                item3 = ref _storage3 == null ? ref ArraySingle<C3>.value[0] : ref _storage3[record.tableRow],
                item4 = ref _storage4 == null ? ref ArraySingle<C4>.value[0] : ref _storage4[record.tableRow],
                item5 = ref _storage5 == null ? ref ArraySingle<C5>.value[0] : ref _storage5[record.tableRow],
                item6 = ref _storage6 == null ? ref ArraySingle<C6>.value[0] : ref _storage6[record.tableRow],
                item7 = ref _storage7 == null ? ref ArraySingle<C7>.value[0] : ref _storage7[record.tableRow],
                item8 = ref _storage8 == null ? ref ArraySingle<C8>.value[0] : ref _storage8[record.tableRow],
            };
        }
    }

    public void TrySetStorageAndOptional<T>(Archetype archetype, int componentIndex, ref T[]? storage) where T : struct
    {
        var terms = SetCorrectTerms(_terms[componentIndex], archetype, componentIndex);
        archetype.TryGetStorage(terms[^1], out storage);
        terms.RemoveAt(terms.Count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationships(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}