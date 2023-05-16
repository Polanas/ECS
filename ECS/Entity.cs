using System.Runtime.CompilerServices;

namespace ECS;


public unsafe struct EntityWithComponent<T> where T : struct
{
    public ref T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (_lastArchetypeId != (*(EntityRecord*)_record).archetypeId)
            {
                ref var record = ref Unsafe.AsRef<EntityRecord>(_record);
                var newTable = _archetypes.GetArchetypeFromRecord(ref record).Table;
                if (newTable == _lastTable)
                    return ref Unsafe.AsRef<T>(_value);

                SetCurrentComponentReference();
            }

            return ref Unsafe.AsRef<T>(_value);
        }
    }

    private readonly Entity entity;
    private void* _value;
    private readonly Archetypes _archetypes;
    private void* _record;
    private int _lastArchetypeId;
    private Table _lastTable;

    public EntityWithComponent(Entity entity, Archetypes archetypes, ref T value)
    {
        ref var record = ref archetypes.GetEntityRecord(entity);
        this.entity = entity;
        _record = Unsafe.AsPointer(ref record);
        _archetypes = archetypes;
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetypes.GetArchetypeFromRecord(ref record).Table;
        _value = Unsafe.AsPointer(ref value);
    }

    private void SetCurrentComponentReference()
    {
        ref var record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetypeFromRecord(ref record);
        var storage = archetype.GetStorage<T>();
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetype.Table;
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