namespace ECS;

public struct AllEntitiesEnumerator
{
    private readonly EntityRecord[] _entityRecords = null!;
    private Archetypes _archetypes;
    private readonly int _entitiesAmount;
    private int _currentEntityIndex = 2;

    public AllEntitiesEnumerator(Archetypes archetypes, EntityRecord[] entityRecords, int entitiesAmount)
    {
        _archetypes = archetypes;
        _entityRecords = entityRecords;
        _entitiesAmount = entitiesAmount;
    }

    public bool MoveNext()
    {
        if (_currentEntityIndex >= _entitiesAmount)
            return false;

        Current = new(_entityRecords[_currentEntityIndex].entity, _archetypes.World);
        _currentEntityIndex++;

        return true;
    }

    public Entity Current { get; private set; }
}