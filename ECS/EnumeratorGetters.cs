namespace ECS;

public readonly struct AllEntitiesEnumeratorGetter
{
    private readonly EntityRecord[] _entityRecords;
    private readonly int _entitiesAmount;
    private readonly Archetypes _archetypes = null!;

    public AllEntitiesEnumeratorGetter(Archetypes archetypes, EntityRecord[] entityRecords, int entitiesAmount)
    {
        _entitiesAmount = entitiesAmount;
        _entityRecords = entityRecords;
        _archetypes = archetypes;
    }

    public AllEntitiesEnumerator GetEnumerator() => new(_archetypes, _entityRecords, _entitiesAmount);
}

public readonly struct SingleEnumeratorGetter<T> where T : struct
{
    private readonly Archetype? _archetype;
    private readonly Archetypes _archetypes;

    public SingleEnumeratorGetter(Archetypes archetypes, Archetype? archetype)
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