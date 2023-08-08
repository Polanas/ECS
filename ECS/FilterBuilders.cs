namespace ECS;


public readonly struct FilterBuilder
{

    internal readonly Mask mask;
    internal readonly PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter> _createFilter =
        (filterData) => new Filter(
            filterData.archetypes,
            filterData.mask,
            filterData.archetypesList,
            filterData.optionalFlags);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
    }

    public FilterBuilder All<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder All<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder All<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder All(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder Any<T>()
       where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder None<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNone(type);
        return this;
    }

    public FilterBuilder Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNone(relationship);
        return this;
    }

    public FilterBuilder None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNone(relationship);
        return this;
    }

    public FilterBuilder None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNone(relationship);
        return this;
    }

    public FilterBuilder None(Entity tag)
    {
        mask.AddNone(tag);
        return this;
    }

    public Filter Build()
    {
        return _arhcetypes.GetFilter(mask, null, _createFilter, optionalFlags);
    }
}

public struct FilterBuilder<C>
    where C : struct
{
    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[1];
    //this HAS to be non-readonly, or the value won't be copied (without any erorrs >:()
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C>> _createFilter =
        (filterData) => new Filter<C>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        None<Prefab>();
        All<C>();
    }

    public FilterBuilder<C> All<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C> All<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> All<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> All(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> Any<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C> None<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Term<C> Term1()
    {
        return new Term<C>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C>(), 0);
    }

    public Filter<C> Build()
    {
        FillMask();
        var filter = (Filter<C>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2>
    where C1 : struct
    where C2 : struct
{
    internal readonly Mask mask;
    internal ListMask listMask;
    internal IterationMode[] iterationsModes = new IterationMode[2];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2>> _createFilter =
       (filterData) => new Filter<C1, C2>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2> Term1()
    {
        return new Term<C1, C2>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2> Term2()
    {
        return new Term<C1, C2>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2, C3>
    where C1 : struct
    where C2 : struct
    where C3 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[3];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3>> _createFilter =
         (filterData) => new Filter<C1, C2, C3>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3> Term1()
    {
        return new Term<C1, C2, C3>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3> Term2()
    {
        return new Term<C1, C2, C3>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3> Term3()
    {
        return new Term<C1, C2, C3>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2, C3, C4>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[4];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3, C4>> _createFilter =
          (filterData) => new Filter<C1, C2, C3, C4>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>().All<C4>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3, C4> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3, C4>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3, C4> Term1()
    {
        return new Term<C1, C2, C3, C4>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3, C4> Term2()
    {
        return new Term<C1, C2, C3, C4>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3, C4> Term3()
    {
        return new Term<C1, C2, C3, C4>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    public Term<C1, C2, C3, C4> Term4()
    {
        return new Term<C1, C2, C3, C4>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C4>(), 3);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2, C3, C4, C5>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[5];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3, C4, C5>> _createFilter =
         (filterData) => new Filter<C1, C2, C3, C4, C5>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>().All<C4>().All<C5>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3, C4, C5>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3, C4, C5> Term1()
    {
        return new Term<C1, C2, C3, C4, C5>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3, C4, C5> Term2()
    {
        return new Term<C1, C2, C3, C4, C5>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3, C4, C5> Term3()
    {
        return new Term<C1, C2, C3, C4, C5>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    public Term<C1, C2, C3, C4, C5> Term4()
    {
        return new Term<C1, C2, C3, C4, C5>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C4>(), 3);
    }

    public Term<C1, C2, C3, C4, C5> Term5()
    {
        return new Term<C1, C2, C3, C4, C5>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C5>(), 4);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2, C3, C4, C5, C6>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[6];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3, C4, C5, C6>> _createFilter =
          (filterData) => new Filter<C1, C2, C3, C4, C5, C6>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>().All<C4>().All<C5>().All<C6>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3, C4, C5, C6>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3, C4, C5, C6> Term1()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3, C4, C5, C6> Term2()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3, C4, C5, C6> Term3()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    public Term<C1, C2, C3, C4, C5, C6> Term4()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C4>(), 3);
    }

    public Term<C1, C2, C3, C4, C5, C6> Term5()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C5>(), 4);
    }

    public Term<C1, C2, C3, C4, C5, C6> Term6()
    {
        return new Term<C1, C2, C3, C4, C5, C6>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C6>(), 5);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public struct FilterBuilder<C1, C2, C3, C4, C5, C6, C7>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly IterationMode[] iterationsModes = new IterationMode[7];
    internal PackedBoolInt optionalFlags;

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3, C4, C5, C6, C7>> _createFilter =
            (filterData) => new Filter<C1, C2, C3, C4, C5, C6, C7>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>().All<C4>().All<C5>().All<C6>().All<C7>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6, C7> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3, C4, C5, C6, C7>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term1()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term2()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term3()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term4()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C4>(), 3);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term5()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C5>(), 4);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term6()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C6>(), 5);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7> Term7()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C7>(), 6);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}

public readonly struct FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{

    internal readonly Mask mask;
    internal readonly ListMask listMask;
    internal readonly PackedBoolInt optionalFlags;
    internal readonly IterationMode[] iterationsModes = new IterationMode[8];

    private readonly Archetypes _arhcetypes;
    private static readonly Func<FilterData, Filter<C1, C2, C3, C4, C5, C6, C7, C8>> _createFilter =
             (filterData) => new Filter<C1, C2, C3, C4, C5, C6, C7, C8>(filterData);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        listMask = ListMaskPool.Get();

        All<C1>().All<C2>().All<C3>().All<C4>().All<C5>().All<C6>().All<C7>().All<C8>();
        None<Prefab>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> All(Entity tag)
    {
        listMask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        listMask.AddNone(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any(Entity tag)
    {
        listMask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        listMask.AddNone(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None(Entity tag)
    {
        listMask.AddNone(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6, C7, C8> Build()
    {
        FillMask();
        var filter = (Filter<C1, C2, C3, C4, C5, C6, C7, C8>)_arhcetypes.GetFilter(
            mask,
            listMask,
            _createFilter,
            optionalFlags,
            iterationsModes,
            listMask.allTypes);
        ListMaskPool.Add(listMask);

        return filter;
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term1()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C1>(), 0);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term2()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C2>(), 1);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term3()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C3>(), 2);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term4()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C4>(), 3);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term5()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C5>(), 4);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term6()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C6>(), 5);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term7()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C7>(), 6);
    }

    public Term<C1, C2, C3, C4, C5, C6, C7, C8> Term8()
    {
        return new Term<C1, C2, C3, C4, C5, C6, C7, C8>(this, _arhcetypes, _arhcetypes.GetComponentIndex<C8>(), 7);
    }

    private void FillMask()
    {
        foreach (var allType in listMask.allTypes)
            mask.AddAll(allType);
        foreach (var anyType in listMask.anyTypes)
            mask.AddAny(anyType);
        foreach (var noneType in listMask.noneTypes)
            mask.AddNone(noneType);
    }
}