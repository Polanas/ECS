namespace ECS;

public readonly struct Term<C>
    where C : struct
{
    private readonly FilterBuilder<C> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C> Optional()
    {
        _filterBuilder.listMask.allTypes.Remove(_component);
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public struct Term<C1, C2>
    where C1 : struct
    where C2 : struct
{
    private FilterBuilder<C1, C2> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}


public readonly struct Term<C1, C2, C3>
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    private readonly FilterBuilder<C1, C2, C3> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public readonly struct Term<C1, C2, C3, C4>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    private readonly FilterBuilder<C1, C2, C3, C4> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3, C4> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3, C4> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public readonly struct Term<C1, C2, C3, C4, C5>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    private readonly FilterBuilder<C1, C2, C3, C4, C5> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3, C4, C5> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public readonly struct Term<C1, C2, C3, C4, C5, C6>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    private readonly FilterBuilder<C1, C2, C3, C4, C5, C6> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3, C4, C5, C6> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public readonly struct Term<C1, C2, C3, C4, C5, C6, C7>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    private readonly FilterBuilder<C1, C2, C3, C4, C5, C6, C7> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3, C4, C5, C6, C7> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}

public readonly struct Term<C1, C2, C3, C4, C5, C6, C7, C8>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    private readonly FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> _filterBuilder;
    private readonly Archetypes _archetypes;
    private readonly ulong _component;
    private readonly int _componentIndex;

    public Term(FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> filterBuilder, Archetypes archetypes, ulong component, int comopnentIndex)
    {
        _archetypes = archetypes;
        _filterBuilder = filterBuilder;
        _component = component;
        _componentIndex = comopnentIndex;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> First<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Second<T>() where T : struct
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(_archetypes.GetComponentIndex<T>());
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> First(Entity entity)
    {
        var first = IdConverter.GetFirst(entity.value);
        var second = IdConverter.GetFirst(_component);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Second(Entity entity)
    {
        var first = IdConverter.GetFirst(_component);
        var second = IdConverter.GetFirst(entity.value);
        var relationship = IdConverter.Compose(first, second, true);

        _filterBuilder.listMask.allTypes[_componentIndex] = relationship;
        return _filterBuilder;
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Optional()
    {
        _filterBuilder.optionalFlags.Set(_componentIndex, true);
        return _filterBuilder;
    }
}