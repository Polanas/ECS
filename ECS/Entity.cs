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
                {
                    var storage = _lastTable.GetStorage<T>();
                    return ref storage[record.tableRow];
                }

                SetCurrentComponentReference();
            }

            ref var record1 = ref Unsafe.AsRef<EntityRecord>(_record);
            var storage1 = _lastTable.GetStorage<T>(_component);
            return ref storage1[record1.tableRow];
        }
    }

    private readonly Entity _entity;
    private readonly ulong _component;
    private readonly Archetypes _archetypes;
    private void* _record;
    private int _lastArchetypeId;
    private Table _lastTable;

    public EntityWithComponent(Entity entity, ulong component, Archetypes archetypes)
    {
        ref var record = ref archetypes.GetEntityRecord(entity);
        _entity = entity;
        _component = component;
        _record = Unsafe.AsPointer(ref record);
        _archetypes = archetypes;
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetypes.GetArchetypeFromRecord(ref record).Table;
    }

    private void SetCurrentComponentReference()
    {
        ref var record = ref _archetypes.GetEntityRecord(_entity);
        var archetype = _archetypes.GetArchetypeFromRecord(ref record);
        _lastArchetypeId = record.archetypeId;
        _lastTable = archetype.Table;
        _record = Unsafe.AsPointer(ref record);
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