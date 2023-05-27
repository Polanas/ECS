using System.Runtime.CompilerServices;

namespace ECS;

public struct EntityWithComponent<T> where T : struct
{
    public ref T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ref var record = ref _records[_recordIndex];

            if (_lastArchetypeId != record.archetypeId)
            {
                var newTable = _archetypes.GetArchetypeFromRecord(ref record).Table;
                if (newTable == _lastTable)
                    return ref _storage[record.tableRow];

                SetCurrentReference(ref record);
            }

            return ref _storage[record.tableRow];
        }
    }

    private readonly ulong _component;
    private readonly Archetypes _archetypes;
    private readonly EntityRecord[] _records;
    private readonly uint _recordIndex;
    private T[] _storage = null!;
    private int _lastArchetypeId;
    private Table _lastTable;

    public EntityWithComponent(Entity entity, ulong component, Archetypes archetypes)
    {
        ref var record = ref archetypes.GetEntityRecord(entity);
        _records = archetypes.EntityRecrods;
        _recordIndex = IdConverter.GetFirst(entity.value);
        _component = component;
        _archetypes = archetypes;
        _lastArchetypeId = record.archetypeId;

        var table = archetypes.GetArchetypeFromRecord(ref record).Table;
        _lastTable = table;
        _storage = table.GetStorage<T>(component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetCurrentReference(ref EntityRecord record)
    {
        var archetype = _archetypes.GetArchetypeFromRecord(ref record);
        _storage = archetype.GetStorage<T>(_component);
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetype.Table;
    }
}

public readonly struct Entity
{
    public static Entity Wildcard { get; } = IdConverter.Compose(uint.MaxValue, uint.MaxValue, false);

    public readonly ulong value;
    public readonly ECSWorld world;

    public Entity(ulong value, ECSWorld world)
    {
        this.value = value;
        this.world = world;
    }

    public static bool operator ==(Entity entity, Entity otherEntity) => entity.value == otherEntity.value;

    public static bool operator !=(Entity entity, Entity otherEntity) => entity.value != otherEntity.value;

    public static implicit operator ulong(Entity entity) => entity.value;

    public static implicit operator Entity(ulong value) => new(value, null!);

    public override bool Equals(object? obj) =>
        ((Entity)obj!).value == value;

    public override int GetHashCode() =>
        value.GetHashCode();
}