using System.Runtime.CompilerServices;

namespace ECS;


public unsafe struct EntityWithComponent<T> where T : struct
{
    public ref T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ref var record = ref Unsafe.AsRef<EntityRecord>(_record);

            if (_lastArchetypeId != record.archetypeId)
            {
                var newTable = _archetypes.GetArchetypeFromRecord(ref record).Table;
                if (newTable == _lastTable)
                    return ref Unsafe.AsRef<T>(_value);

                SetCurrentComponentReference(ref record);
            }

            return ref Unsafe.AsRef<T>(_value);
        }
    }

    private readonly ulong _component;
    private readonly Archetypes _archetypes;
    private void* _record;
    private void* _value;
    private int _lastArchetypeId;
    private Table _lastTable;

    public EntityWithComponent(Entity entity, ulong component, Archetypes archetypes, ref T value)
    {
        _value = Unsafe.AsPointer(ref value);
        ref var record = ref archetypes.GetEntityRecord(entity);
        _entity = entity;
        _component = component;
        _record = Unsafe.AsPointer(ref record);
        _archetypes = archetypes;
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetypes.GetArchetypeFromRecord(ref record).Table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetCurrentComponentReference(ref EntityRecord record)
    {
        var archetype = _archetypes.GetArchetypeFromRecord(ref record);
        var storage = archetype.GetStorage<T>(_component);
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetype.Table;
        _record = Unsafe.AsPointer(ref record);
        _value = Unsafe.AsPointer(ref storage[record.tableRow]);
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