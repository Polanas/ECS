namespace ECS;

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