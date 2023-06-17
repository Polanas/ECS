using System.Runtime.CompilerServices;

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

    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList)
    {
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

public class Filter<C> : Filter
    where C : struct
{
    internal readonly ulong[] terms;

    public Filter(
       Archetypes archetypes,
       Mask mask, List<Archetype> archetypesList,
       List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(archetypes, archetypesList, terms);
    }
}

public class Filter<C1, C2> : Filter
    where C1 : struct
    where C2 : struct
{
    internal readonly ulong[] terms;

    public Filter(
        Archetypes archetypes,
        Mask mask,
        List<Archetype> archetypesList,
        List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(archetypes, archetypesList, terms);
    }
}

public class Filter<C1, C2, C3> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    internal readonly ulong[] terms;

    public Filter(
        Archetypes archetypes,
        Mask mask, List<Archetype> archetypesList,
        List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(archetypes, archetypesList, terms);
    }
}

public class Filter<C1, C2, C3, C4> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    internal readonly ulong[] terms;

    public Filter(
      Archetypes archetypes,
      Mask mask, List<Archetype> archetypesList,
      List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(archetypes, archetypesList, terms);
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

    public Filter(
      Archetypes archetypes,
      Mask mask, List<Archetype> archetypesList,
      List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(archetypes, archetypesList, terms);
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

    public Filter(
       Archetypes archetypes,
       Mask mask, List<Archetype> archetypesList,
       List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(archetypes, archetypesList, terms);
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

    public Filter(
       Archetypes archetypes,
       Mask mask, List<Archetype> archetypesList,
       List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(archetypes, archetypesList, terms);
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

    public Filter(
       Archetypes archetypes,
       Mask mask, List<Archetype> archetypesList,
       List<ulong> terms) : base(archetypes, mask, archetypesList)
    {
        this.terms = terms.ToArray();
    }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, archetypesList, terms);
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

        UpdateStorage();
    }

    public void UpdateStorage()
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

        UpdateStorage();
    }

    public void Dispose() => _archetypes.Unlock();

    public Entry<C> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item = ref _storage is null ?
                   ref Unsafe.As<C[]>(_entityStorage)[0] :
                   ref _storage[_entityIndex]
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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C[] _storage = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _archetypes = archetypes;
        _terms = terms;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }

    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage = archetype.GetStorage<C>(GetCorrectTerm(_terms[0], archetype));
        _entityStorage = archetype.Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
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

    public Entry<C> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item = ref _storage[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}

public struct Enumerator<C1, C2> : IDisposable
    where C1 : struct
    where C2 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private readonly ulong[] _terms;
    private readonly ECSWorld _world;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
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

        var archetype = _archetypesList[_archetypeIndex];
        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _entityStorage = archetype.Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
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

    public Entry<C1, C2> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private readonly ECSWorld _world;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _entityStorage = archetype.Entities;
    }

    public bool MoveNext()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return false;

        var archetype = _archetypesList[_archetypeIndex];

        if (++_entityIndex < archetype.Count)
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

    public Entry<C1, C2, C3> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _storage4 = archetype.GetStorage<C4>(GetCorrectTerm(_terms[3], archetype));
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
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex],
            item4 = ref _storage4[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _storage4 = archetype.GetStorage<C4>(GetCorrectTerm(_terms[3], archetype));
        _storage5 = archetype.GetStorage<C5>(GetCorrectTerm(_terms[4], archetype));
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
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex],
            item4 = ref _storage4[_entityIndex],
            item5 = ref _storage5[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;
    private C6[] _storage6 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _storage4 = archetype.GetStorage<C4>(GetCorrectTerm(_terms[3], archetype));
        _storage5 = archetype.GetStorage<C5>(GetCorrectTerm(_terms[4], archetype));
        _storage6 = archetype.GetStorage<C6>(GetCorrectTerm(_terms[5], archetype));
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
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex],
            item4 = ref _storage4[_entityIndex],
            item5 = ref _storage5[_entityIndex],
            item6 = ref _storage6[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;
    private C6[] _storage6 = null!;
    private C7[] _storage7 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _archetypes = archetypes;
        _terms = terms;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];
        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _storage4 = archetype.GetStorage<C4>(GetCorrectTerm(_terms[3], archetype));
        _storage5 = archetype.GetStorage<C5>(GetCorrectTerm(_terms[4], archetype));
        _storage6 = archetype.GetStorage<C6>(GetCorrectTerm(_terms[5], archetype));
        _storage7 = archetype.GetStorage<C7>(GetCorrectTerm(_terms[6], archetype));
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
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex],
            item4 = ref _storage4[_entityIndex],
            item5 = ref _storage5[_entityIndex],
            item6 = ref _storage6[_entityIndex],
            item7 = ref _storage7[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;
    private C6[] _storage6 = null!;
    private C7[] _storage7 = null!;
    private C8[] _storage8 = null!;

    public Enumerator(Archetypes archetypes, List<Archetype> archetypesList, ulong[] terms)
    {
        _archetypesList = archetypesList;
        _terms = terms;
        _archetypes = archetypes;
        _entityIndex = -1;
        _world = archetypes.World;
        archetypes.Lock();

        UpdateStorage();
    }
    public void UpdateStorage()
    {
        if (_archetypeIndex == _archetypesList.Count)
            return;

        var archetype = _archetypesList[_archetypeIndex];

        _storage1 = archetype.GetStorage<C1>(GetCorrectTerm(_terms[0], archetype));
        _storage2 = archetype.GetStorage<C2>(GetCorrectTerm(_terms[1], archetype));
        _storage3 = archetype.GetStorage<C3>(GetCorrectTerm(_terms[2], archetype));
        _storage4 = archetype.GetStorage<C4>(GetCorrectTerm(_terms[3], archetype));
        _storage5 = archetype.GetStorage<C5>(GetCorrectTerm(_terms[4], archetype));
        _storage6 = archetype.GetStorage<C6>(GetCorrectTerm(_terms[5], archetype));
        _storage7 = archetype.GetStorage<C7>(GetCorrectTerm(_terms[6], archetype));
        _storage8 = archetype.GetStorage<C8>(GetCorrectTerm(_terms[7], archetype));
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
        get => new()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _storage1[_entityIndex],
            item2 = ref _storage2[_entityIndex],
            item3 = ref _storage3[_entityIndex],
            item4 = ref _storage4[_entityIndex],
            item5 = ref _storage5[_entityIndex],
            item6 = ref _storage6[_entityIndex],
            item7 = ref _storage7[_entityIndex],
            item8 = ref _storage8[_entityIndex]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCorrectTerm(ulong term, Archetype archetype)
    {
        var correctTerm = term;

        bool relationIsWildcard = IdConverter.GetFirst(term) == Archetypes.wildCard32;
        bool targetIsWildcard = IdConverter.GetSecond(term) == Archetypes.wildCard31;

        if (relationIsWildcard || targetIsWildcard)
            correctTerm = _archetypes.FindRelationship(archetype, term, relationIsWildcard);

        return correctTerm;
    }
}