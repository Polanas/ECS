namespace ECS;

public interface IOnComponentActionSystem
{
    void OnComponentAdd(Entity entity);
    void OnComponentRemove(Entity entity);
}

internal readonly struct ComponentStorage
{
    public Type Type { get; init; }

    public ComponentStorage(Type type)
    {
        Type = type;
    }
}

internal readonly struct RelationshipStorage
{
    public Type TargetType { get; init; }
    public Type RelationType { get; init; }

    public RelationshipStorage(Type relationType, Type targetType)
    {
        TargetType = targetType;
        RelationType = relationType;
    }
}

public abstract class OnComponentActionSystem : IOnComponentActionSystem, ISystem
{
    internal SortedSet<ulong> allComponents = null!;
    internal HashSet<ulong> allComponentsHashset = null!;
    internal SortedSet<ulong> noneComponents = null!;
    internal HashSet<ulong> noneComponentsHashset = null!;
    internal SortedSet<ulong> anyComponents = null!;
    internal HashSet<ulong> anyComponentsHashset = null!;

    public OnComponentActionSystem()
    {
        allComponents = new();
        noneComponents = new();
        anyComponents = new();

        anyComponentsHashset = new();
        allComponentsHashset = new();
        noneComponentsHashset = new();
    }

    public virtual void OnComponentAdd(Entity entity) { }
    public virtual void OnComponentRemove(Entity entity) { }

    public void All<T>() where T : struct
    {
        var component = ECSWorld.Instance!.IndexOf<T>();
        allComponents.Add(component);
        allComponentsHashset.Add(component);

        AddAll(component);
    }

    public void All<T1, T2>() where T1 : struct where T2 : struct
    {
        var world = ECSWorld.Instance!;

        var relationId = IdConverter.GetFirst(world.IndexOf<T1>());
        var targetId = IdConverter.GetFirst(world.IndexOf<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);

        AddAll(relationship);
    }

    public void None<T>() where T : struct
    {
        var component = ECSWorld.Instance!.IndexOf<T>();
        AddNone(component);
    }

    public void None<T1, T2>() where T1 : struct where T2 : struct
    {
        var world = ECSWorld.Instance!;

        var relationId = IdConverter.GetFirst(world.IndexOf<T1>());
        var targetId = IdConverter.GetFirst(world.IndexOf<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        AddNone(relationship);
    }

    public void Any<T>() where T : struct
    {
        var component = ECSWorld.Instance!.IndexOf<T>();
        AddAny(component);
    }

    public void Any<T1, T2>() where T1 : struct where T2 : struct
    {
        var world = ECSWorld.Instance!;

        var relationId = IdConverter.GetFirst(world.IndexOf<T1>());
        var targetId = IdConverter.GetFirst(world.IndexOf<T2>());

        var relationship = IdConverter.Compose(relationId, targetId, true);
        AddAny(relationship);
    }

    private void AddAll(ulong component)
    {
        allComponents.Add(component);
        allComponentsHashset.Add(component);
    }

    private void AddNone(ulong component)
    {
        noneComponents.Add(component);
        noneComponentsHashset.Add(component);
    }

    private void AddAny(ulong component)
    {
        anyComponents.Add(component);
        anyComponentsHashset.Add(component);
    }
}