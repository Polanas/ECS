namespace ECS;

public interface IAll { }

public interface INone { }

public interface IAny { }

public interface IInjectedFilterParameter
{
    ulong GetComponent();
}

public struct All<T> : IInjectedFilterParameter, IAll where T : struct
{
    public ulong index;

    public All()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

public struct All<T1, T2> : IInjectedFilterParameter, IAll where T1 : struct where T2 : struct
{
    public ulong index;

    public All()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct None<T> : IInjectedFilterParameter, INone where T : struct
{
    public ulong index;

    public None()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

public struct None<T1, T2> : IInjectedFilterParameter, INone where T1 : struct where T2 : struct
{
    public ulong index;

    public None()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct Any<T> : IInjectedFilterParameter, IAny where T : struct
{
    public ulong index;

    public Any()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

public struct Any<T1, T2> : IInjectedFilterParameter, IAny where T1 : struct where T2 : struct
{
    public ulong index;

    public Any()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct FilterInject : IDataInject
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter value = null!;

    private FilterBuilder _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(value.Archetypes, value.archetypesList);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C> : IDataInject
    where C : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C> value = null!;

    private FilterBuilder<C> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2> : IDataInject
    where C1 : struct
    where C2 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2> value = null!;

    private FilterBuilder<C1, C2> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3> value = null!;

    private FilterBuilder<C1, C2, C3> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3, C4> value = null!;

    private FilterBuilder<C1, C2, C3, C4> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3, C4, C5> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3, C4, C5, C6> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6, C7> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3, C4, C5, C6, C7> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6, C7, C8> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    public int Count => value.Count;
    public bool HasEntities => value.HasEntities;
    public Filter<C1, C2, C3, C4, C5, C6, C7, C8> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.allTypes);
        AddTypes(_notTypes, _builder.mask.noneTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7, C8> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7, C8>(value.Archetypes, value.archetypesList, value.terms, value);
    }

    private static void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}