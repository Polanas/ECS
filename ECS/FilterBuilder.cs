namespace ECS;


public struct FilterBuilder
{

    internal readonly Mask mask;

    private readonly Archetypes _arhcetypes;
    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter(archetypes, mask, archetypesList);

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
        mask.AddNot(type);
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
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter Build()
    {
        return _arhcetypes.GetFilter(mask, _createFilter);
    }
}

public struct FilterBuilder<C>
    where C : struct
{

    internal readonly Mask mask;
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();

        AddAll<C>();
    }

    public FilterBuilder<C> All<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C> All<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> All<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> All(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAll(relationship);
        return this;
    }

    public FilterBuilder<C> Any<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C> None<T>() where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C> Build()
    {
        return (Filter<C>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
    }
}

public struct FilterBuilder<C1, C2>
    where C1 : struct
    where C2 : struct
{

    internal readonly Mask mask;
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>();
    }

    public FilterBuilder<C1, C2> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2> Build()
    {
        return (Filter<C1, C2>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
    }
}

public struct FilterBuilder<C1, C2, C3>
    where C1 : struct
    where C2 : struct
    where C3 : struct
{

    internal readonly Mask mask;
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>();
    }

    public FilterBuilder<C1, C2, C3> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3> Build()
    {
        return (Filter<C1, C2, C3>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
    }
}

public struct FilterBuilder<C1, C2, C3, C4>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{

    internal readonly Mask mask;
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3, C4>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>().AddAll<C4>();
    }

    public FilterBuilder<C1, C2, C3, C4> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4> Build()
    {
        return (Filter<C1, C2, C3, C4>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3, C4> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
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
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3, C4, C5>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>().AddAll<C4>().AddAll<C5>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5> Build()
    {
        return (Filter<C1, C2, C3, C4, C5>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3, C4, C5> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
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
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3, C4, C5, C6>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>().AddAll<C4>().AddAll<C5>().AddAll<C6>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6> Build()
    {
        return (Filter<C1, C2, C3, C4, C5, C6>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3, C4, C5, C6> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
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
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3, C4, C5, C6, C7>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>().AddAll<C4>().AddAll<C5>().AddAll<C6>().AddAll<C7>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6, C7> Build()
    {
        return (Filter<C1, C2, C3, C4, C5, C6, C7>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
    }
}

public struct FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8>
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
    private readonly Archetypes _arhcetypes;

    private static Func<Archetypes, Mask, List<Archetype>, Filter> _createFilter =
        (archetypes, mask, archetypesList) => new Filter<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, mask, archetypesList);

    public FilterBuilder(Archetypes archetypes)
    {
        _arhcetypes = archetypes;
        mask = MaskPool.Get();
        AddAll<C1>().AddAll<C2>().AddAll<C3>().AddAll<C4>().AddAll<C5>().AddAll<C6>().AddAll<C7>().AddAll<C8>();
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> All<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAll(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> All(Entity tag)
    {
        mask.AddAll(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddAny(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T>()
        where T : struct
    {
        var type = _arhcetypes.GetComponentIndex<T>();
        mask.AddNot(type);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any(Entity tag)
    {
        mask.AddAny(tag);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T1, T2>()
     where T1 : struct
     where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddAny(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T>(Entity target) where T : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T>());
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None<T1, T2>() where T1 : struct where T2 : struct
    {
        var relationId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T1>());
        var targetId = IdConverter.GetFirst(_arhcetypes.GetComponentIndex<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);

        var relationship = IdConverter.Compose(relationId, targetId, true);
        mask.AddNot(relationship);
        return this;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> None(Entity tag)
    {
        mask.AddNot(tag);
        return this;
    }

    public Filter<C1, C2, C3, C4, C5, C6, C7, C8> Build()
    {
        return (Filter<C1, C2, C3, C4, C5, C6, C7, C8>)_arhcetypes.GetFilter(mask, _createFilter);
    }

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> AddAll<T>() where T : struct
    {
        if (PairHelper.IsPair<T>())
            All(PairHelper.GetRelationship<T>());
        else All<T>();

        return this;
    }
}