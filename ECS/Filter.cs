using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace ECS;

public interface IPair { }
public interface IPair1 { }
public interface IPair2 { }

public struct Pair1<T1, T2> : IPair1, IPair where T1 : struct where T2 : struct
{
    internal T1 value;
    internal ulong componentIndex1;
    internal ulong componentIndex2;
    internal int size;

    public Pair1()
    {
        var world = ECSWorld.Instance!;
        componentIndex1 = world.Archetypes.GetComponentIndex<T1>();
        componentIndex2 = world.Archetypes.GetComponentIndex<T2>();
        size = Unsafe.SizeOf<T1>();

#if DEBUG
        CheckIfUnmanaged();
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T1 GetValue()
    {
        unsafe
        {
            fixed (T1* p = &value)
                return ref *p;
        }
    }

#if DEBUG
    private void CheckIfUnmanaged()
    {
        var type = typeof(T1);

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T1>())
            throw new Exception($"{type.Name} is unmanaged; _data relationship cannot contain unmanaged allComponents");
    }
#endif
}

public struct Pair2<T1, T2> : IPair2, IPair where T1 : struct where T2 : struct
{
    internal T2 value;
    internal ulong componentIndex1;
    internal ulong componentIndex2;
    internal int size;

    public Pair2()
    {
        var world = ECSWorld.Instance!;
        componentIndex1 = world.Archetypes.GetComponentIndex<T1>();
        componentIndex2 = world.Archetypes.GetComponentIndex<T2>();
        size = Unsafe.SizeOf<T2>();

#if DEBUG
        CheckIfUnmanaged();
#endif
    }

    public ref T2 GetValue()
    {
        unsafe
        {
            fixed (T2* p = &value)
                return ref *p;
        }
    }

#if DEBUG
    private void CheckIfUnmanaged()
    {
        var type = typeof(T2);

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T2>())
            throw new Exception($"{type.Name} is unmanaged; _data relationship cannot contain unmanaged allComponents");
    }
#endif
}

public class Filter
{
    public Mask Mask => _mask;
    public Archetypes Archetypes => archetypes;

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
            throw new Exception("Cannot get first entity as no entites satisfy the filter.");
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
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(archetypes, archetypesList);
    }
}

public class Filter<C1, C2> : Filter
    where C1 : struct
    where C2 : struct
{
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }


    public new Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(archetypes, archetypesList);
    }
}

public class Filter<C1, C2, C3> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(archetypes, archetypesList);
    }
}

public class Filter<C1, C2, C3, C4> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(archetypes, archetypesList);
    }
}

public class Filter<C1, C2, C3, C4, C5> : Filter
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(archetypes, archetypesList);
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
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(archetypes, archetypesList);
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
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(archetypes, archetypesList);
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
    public Filter(Archetypes archetypes, Mask mask, List<Archetype> archetypesList) : base(archetypes, mask, archetypesList) { }

    public new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, archetypesList);
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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private ECSWorld _world;

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
        get => new Entry
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
        };
    }

    internal static void SetStorage<T>(Archetype archetype, ref T[] storage, ref PairData pairData) where T : struct
    {
        pairData.isPair = PairHelper.IsPair<T>();

        if (pairData.isPair)
        {
            storage = archetype.GetStorage<T>(PairHelper.GetRelationship<T>());
            pairData.size = PairHelper.GetSize<T>();
        }
        else storage = archetype.GetStorage<T>();
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

    public unsafe Entry<C> Current
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

public unsafe struct Enumerator<C> : IDisposable
    where C : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C[] _storage = null!;
    private PairData _CData;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage, ref _CData);
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
        get => new Entry<C>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item = ref _CData.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage[0], _CData.size * _entityIndex)
                       : ref _storage[_entityIndex]
        };
    }
}

public struct Enumerator<C1, C2> : IDisposable
    where C1 : struct
    where C2 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private PairData _C1Data;
    private PairData _C2Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
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
        get => new Entry<C1, C2>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                       : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                       : ref _storage2[_entityIndex]
        };
    }
}

public struct Enumerator<C1, C2, C3> : IDisposable
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    private readonly List<Archetype> _archetypesList;
    private readonly Archetypes _archetypes;
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
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
        get => new Entry<C1, C2, C3>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                       : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                       : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                       ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                       : ref _storage3[_entityIndex]
        };
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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private PairData _C4Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
        Enumerator.SetStorage(archetype, ref _storage4, ref _C4Data);
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
        get => new Entry<C1, C2, C3, C4>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                         ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                         : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                         ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                         : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                         ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                         : ref _storage3[_entityIndex],
            item4 = ref _C4Data.isPair
                         ? ref Unsafe.AddByteOffset(ref _storage4[0], _C4Data.size * _entityIndex)
                         : ref _storage4[_entityIndex]
        };
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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private PairData _C4Data;
    private PairData _C5Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
        Enumerator.SetStorage(archetype, ref _storage4, ref _C4Data);
        Enumerator.SetStorage(archetype, ref _storage5, ref _C5Data);
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
        get => new Entry<C1, C2, C3, C4, C5>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                        : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                        : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                        : ref _storage3[_entityIndex],
            item4 = ref _C4Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage4[0], _C4Data.size * _entityIndex)
                        : ref _storage4[_entityIndex],
            item5 = ref _C5Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage5[0], _C5Data.size * _entityIndex)
                        : ref _storage5[_entityIndex]
        };
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
    private int _archetypeIndex;
    private int _entityIndex;
    private ulong[] _entityStorage = null!;
    private C1[] _storage1 = null!;
    private C2[] _storage2 = null!;
    private C3[] _storage3 = null!;
    private C4[] _storage4 = null!;
    private C5[] _storage5 = null!;
    private C6[] _storage6 = null!;
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private PairData _C4Data;
    private PairData _C5Data;
    private PairData _C6Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
        Enumerator.SetStorage(archetype, ref _storage4, ref _C4Data);
        Enumerator.SetStorage(archetype, ref _storage5, ref _C5Data);
        Enumerator.SetStorage(archetype, ref _storage6, ref _C6Data);
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
        get => new Entry<C1, C2, C3, C4, C5, C6>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                        : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                        : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                        : ref _storage3[_entityIndex],
            item4 = ref _C4Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage4[0], _C4Data.size * _entityIndex)
                        : ref _storage4[_entityIndex],
            item5 = ref _C5Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage5[0], _C5Data.size * _entityIndex)
                        : ref _storage5[_entityIndex],
            item6 = ref _C6Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage6[0], _C6Data.size * _entityIndex)
                        : ref _storage6[_entityIndex]
        };
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
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private PairData _C4Data;
    private PairData _C5Data;
    private PairData _C6Data;
    private PairData _C7Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
        Enumerator.SetStorage(archetype, ref _storage4, ref _C4Data);
        Enumerator.SetStorage(archetype, ref _storage5, ref _C5Data);
        Enumerator.SetStorage(archetype, ref _storage6, ref _C6Data);
        Enumerator.SetStorage(archetype, ref _storage7, ref _C7Data);
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
        get => new Entry<C1, C2, C3, C4, C5, C6, C7>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                        : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                        : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                        : ref _storage3[_entityIndex],
            item4 = ref _C4Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage4[0], _C4Data.size * _entityIndex)
                        : ref _storage4[_entityIndex],
            item5 = ref _C5Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage5[0], _C5Data.size * _entityIndex)
                        : ref _storage5[_entityIndex],
            item6 = ref _C6Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage6[0], _C6Data.size * _entityIndex)
                        : ref _storage6[_entityIndex],
            item7 = ref _C7Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage7[0], _C7Data.size * _entityIndex)
                        : ref _storage7[_entityIndex]
        };
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
    private PairData _C1Data;
    private PairData _C2Data;
    private PairData _C3Data;
    private PairData _C4Data;
    private PairData _C5Data;
    private PairData _C6Data;
    private PairData _C7Data;
    private PairData _C8Data;
    private ECSWorld _world;

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

        var archetype = _archetypesList[_archetypeIndex];

        Enumerator.SetStorage(archetype, ref _storage1, ref _C1Data);
        Enumerator.SetStorage(archetype, ref _storage2, ref _C2Data);
        Enumerator.SetStorage(archetype, ref _storage3, ref _C3Data);
        Enumerator.SetStorage(archetype, ref _storage4, ref _C4Data);
        Enumerator.SetStorage(archetype, ref _storage5, ref _C5Data);
        Enumerator.SetStorage(archetype, ref _storage6, ref _C6Data);
        Enumerator.SetStorage(archetype, ref _storage7, ref _C7Data);
        Enumerator.SetStorage(archetype, ref _storage8, ref _C8Data);
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
        get => new Entry<C1, C2, C3, C4, C5, C6, C7, C8>()
        {
            entity = new Entity(_entityStorage[_entityIndex], _world),
            item1 = ref _C1Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage1[0], _C1Data.size * _entityIndex)
                        : ref _storage1[_entityIndex],
            item2 = ref _C2Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage2[0], _C2Data.size * _entityIndex)
                        : ref _storage2[_entityIndex],
            item3 = ref _C3Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage3[0], _C3Data.size * _entityIndex)
                        : ref _storage3[_entityIndex],
            item4 = ref _C4Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage4[0], _C4Data.size * _entityIndex)
                        : ref _storage4[_entityIndex],
            item5 = ref _C5Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage5[0], _C5Data.size * _entityIndex)
                        : ref _storage5[_entityIndex],
            item6 = ref _C6Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage6[0], _C6Data.size * _entityIndex)
                        : ref _storage6[_entityIndex],
            item7 = ref _C7Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage7[0], _C7Data.size * _entityIndex)
                        : ref _storage7[_entityIndex],
            item8 = ref _C8Data.isPair
                        ? ref Unsafe.AddByteOffset(ref _storage8[0], _C8Data.size * _entityIndex)
                        : ref _storage8[_entityIndex]
        };
    }
}