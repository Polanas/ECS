using System.Runtime.CompilerServices;

namespace ECS;

public static class EntityExtensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Copy(this Entity entity) => entity.world.CopyEntity(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add<T>(this Entity entity, T value = default) where T : struct
    {
        if (Unsafe.SizeOf<T>() > 1)
            entity.world.AddComponent(entity, value);
        else entity.world.AddTag<T>(entity);

        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(this Entity entity, Entity parent) => entity.world.GetName(entity, parent);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(this Entity entity) => entity.world.GetName(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Name(this Entity entity, string name)
    {
        entity.world.Name(entity, name);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasName(this Entity entity) => entity.world.HasName(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity ChildOf(this Entity entity, Entity parent)
    {
        var world = entity.world;
        world.AddRelationship<ChildOf>(entity, parent);

        if (world.Archetypes.EntityHasName(entity, 0))
            world.Archetypes.ChangeEntityNameParent(entity, 0, parent);

        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsChildOf(this Entity entity, Entity potentialParent) =>
        entity.world.IsEntityChildOf(entity, potentialParent);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity RemoveChildOf(this Entity entity)
    {
        entity.world.RemoveChildOf(entity);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity InstanceOf(this Entity entity, string instanceName)
    {
        entity.world.AddInstanceOf(entity, entity.world.GetEntity(instanceName));
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity InstanceOf(this Entity entity, Entity instance)
    {
        entity.world.AddInstanceOf(entity, instance);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add(this Entity target, Entity tag)
    {
        target.world.AddTag(target, tag);
        return target;
    }


    public static EntityWithComponent<T> GetOrAdd<T>(this Entity entity, T? value = default) where T : struct
    {
        if (!entity.world.HasComponent<T>(entity))
            Add<T>(entity);

        var component = entity.world.GetComponent<T>(entity);
        if (value != null)
            component.Value = value.Value;

        return component;
    }

    public static Entity GetOrAdd(this Entity entity, Entity tag)
    {
        var world = entity.world;

        if (!world.HasComponent(entity, tag))
            world.AddComponent(entity, tag);

        return entity;
    }

    public static EntityWithComponent<T> GetOrAdd<T>(this Entity entity, Entity target, T? value = default) where T : struct
    {
        var world = entity.world;

        if (!world.HasRelationship<T>(entity, target))
            world.AddRelationship<T>(entity, target);

        var component = world.GetRelationship<T>(entity, target);
        if (value != null)
            component.Value = value.Value;

        return component;
    }

    public static EntityWithComponent<T1> GetOrAdd1<T1, T2>(this Entity entity, T1? value = default) where T1 : struct where T2 : struct
    {
        var world = entity.world;

        if (!world.HasRelationship<T1, T2>(entity))
            world.AddRelationship<T1, T2>(entity, default(T1));

        var component = world.GetRelationship1<T1, T2>(entity);
        if (value != null)
            component.Value = value.Value;

        return component;
    }

    public static EntityWithComponent<T2> GetOrAdd2<T1, T2>(this Entity entity, T2? value = default) where T1 : struct where T2 : struct
    {
        var world = entity.world;

        if (!world.HasRelationship<T1, T2>(entity))
            world.AddRelationship<T1, T2>(entity, default(T2));

        var component = world.GetRelationship2<T1, T2>(entity);
        if (value != null)
            component.Value = value.Value;

        return component;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Get<T>(this Entity entity, out T component) where T : struct
    {
        component = entity.world.GetComponent<T>(entity).Value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Get<T>(this Entity entity, Entity target, out T component) where T : struct
    {
        component = entity.world.GetRelationship<T>(entity, target).Value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityWithComponent<T> Get<T>(this Entity entity) where T : struct =>
        entity.world.GetComponent<T>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityWithComponent<T> Get<T>(this Entity entity, Entity target) where T : struct => entity.world.GetRelationship<T>(entity, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Set<T>(this Entity entity, T value) where T : struct
    {
        entity.world.GetComponent<T>(entity).Value = value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Set<T>(this Entity entity, Entity target, T value) where T : struct
    {
        entity.world.GetRelationship<T>(entity, target).Value = value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has<T>(this Entity entity) where T : struct =>
        entity.world.HasComponent<T>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has(this Entity entity, Entity tag) =>
         entity.world.HasComponent(entity, tag);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Remove(this Entity entity)
    {
        entity.world.RemoveEntity(entity);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Remove<T>(this Entity entity) where T : struct
    {
        entity.world.RemoveComponent<T>(entity);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlive(this Entity entity)
    {
        var world = entity.world;
        if (world == null)
            return false;

        return world.IsAlive(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add(this Entity entity, Entity relation, Entity target)
    {
        entity.world.AddRelationship(entity, relation, target);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has(this Entity entity, Entity relation, Entity target) =>
        entity.world.HasRelationship(entity, relation, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Remove(this Entity entity, Entity relation, Entity target) =>
        entity.world.RemoveRelationship(entity, relation, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add<T>(this Entity entity, Entity target, T value = default) where T : struct
    {
        entity.world.AddRelationship(entity, target, value);
        return entity;
    }

    /// <summary>
    /// This method assumes that both T1 and T2 don't contain any data
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add<T1, T2>(this Entity entity) where T1 : struct where T2 : struct
    {
       entity.world.AddRelationship<T1, T2>(entity);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add<T1, T2>(this Entity entity, T1 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add<T1, T2>(this Entity entity, T2 value) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add1<T1, T2>(this Entity entity, T1 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Add2<T1, T2>(this Entity entity, T2 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Relationship Find<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.FindRelationship<T>(entity, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Relationship Find<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.FindRelationship<T1, T2>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Set1<T1, T2>(this Entity entity, T1 value) where T1 : struct where T2 : struct
    {
        entity.world.GetRelationship1<T1, T2>(entity).Value = value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Set2<T1, T2>(this Entity entity, T2 value) where T1 : struct where T2 : struct
    {
        entity.world.GetRelationship2<T1, T2>(entity).Value = value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Get1<T1, T2>(this Entity entity, out T1 component) where T1 : struct where T2 : struct
    {
        component = entity.world.GetRelationship1<T1, T2>(entity).Value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Entity Get2<T1, T2>(this Entity entity, out T2 component) where T1 : struct where T2 : struct
    {
        component = entity.world.GetRelationship2<T1, T2>(entity).Value;
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityWithComponent<T1> Get1<T1, T2>(this Entity entity) where T1 : struct where T2 : struct => entity.world.GetRelationship1<T1, T2>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityWithComponent<T2> Get2<T1, T2>(this Entity entity) where T1 : struct where T2 : struct => entity.world.GetRelationship2<T1, T2>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.HasRelationship<T1, T2>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Remove<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.RemoveRelationship<T1, T2>(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.HasRelationship<T>(entity, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Remove<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.RemoveRelationship<T>(entity, target);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RelationshipEnumeratorGetter GetRelationships(this Entity entity) =>
        entity.world.GetRelationships(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ComponentEnumeratorGetter GetComponents(this Entity entity) =>
        entity.world.GetComponents(entity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIndexEqual(Entity entity, Entity otherEntity) =>
        IdConverter.GetFirst(entity) == otherEntity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Filter GetChildren(this Entity enitity) => enitity.world.GetChildren(enitity);
}