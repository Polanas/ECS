namespace ECS;

public readonly struct EnumeratorSingleGetter<T> where T : struct
{
    private readonly Archetype? _archetype;
    private readonly Archetypes _archetypes;

    public EnumeratorSingleGetter(Archetypes archetypes, Archetype? archetype)
    {
        _archetype = archetype;
        _archetypes = archetypes;
    }

    public EnumeratorSingle<T> GetEnumerator() => new(_archetypes, _archetype);
}

public readonly struct RelationshipEnumeratorGetter
{
    private readonly Archetype _archetype;

    public RelationshipEnumeratorGetter(Archetype archetype)
    {
        _archetype = archetype;
    }

    public RelationshipEnumerator GetEnumerator() => new(_archetype);
}

public readonly struct ComponentEnumeratorGetter
{
    private readonly Archetype _archetype;

    public ComponentEnumeratorGetter(Archetype archetype)
    {
        _archetype = archetype;
    }

    public ComponentEnumerator GetEnumerator() => new(_archetype);
}