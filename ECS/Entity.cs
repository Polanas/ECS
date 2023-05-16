using System.Runtime.CompilerServices;

namespace ECS;

public ref struct EntityWithComponent<T> where T : struct
{
    public ref T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (_lastArchetypeId != _record.archetypeId)
            {
                var newTable = _archetypes.GetArchetypeFromRecord(ref _record).Table;
                if (newTable == _lastTable)
                    return ref _value;

                SetCurrentComponentReference();
            }

            return ref _value;
        }
    }

    private readonly Entity entity;
    private ref T _value;
    private readonly Archetypes _archetypes;
    private ref EntityRecord _record;
    private int _lastArchetypeId;
    private Table _lastTable;

    public EntityWithComponent(Entity entity, Archetypes archetypes, ref T value)
    {
        _record = ref archetypes.GetEntityRecord(entity);
        this.entity = entity;
        _archetypes = archetypes;
        _lastArchetypeId = _record.archetypeId;
        _lastTable = archetypes.GetArchetypeFromRecord(ref _record).Table;
        _value = ref value;
    }

    private void SetCurrentComponentReference()
    {
        _record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetypeFromRecord(ref _record);
        var storage = archetype.GetStorage<T>();
        _lastArchetypeId = _record.archetypeId;
        _lastTable = archetype.Table;
        _value = ref storage[_record.tableRow];
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