using System.Runtime.CompilerServices;

namespace ECS;

public static class EntityExtensions
{

    public static Entity Copy(this Entity entity) => entity.world.CopyEntity(entity);

    public static Entity Add<T>(this Entity entity, T value = default) where T : struct
    {
        if (Unsafe.SizeOf<T>() > 1)
            entity.world.AddComponent(entity, value);
        else entity.world.AddTag<T>(entity);

        return entity;
    }

    public static string GetName(this Entity entity, Entity parent) => entity.world.GetName(entity, parent);

    public static string GetName(this Entity entity) => entity.world.GetName(entity);

    public static Entity Name(this Entity entity, string name)
    {
        entity.world.Name(entity, name);
        return entity;
    }

    public static bool HasName(this Entity entity) => entity.world.HasName(entity);

    public static Entity ChildOf(this Entity entity, Entity parent)
    {
        var world = entity.world;
        world.AddRelationship<ChildOf>(entity, parent);

        if (world.Archetypes.EntityHasName(entity, 0))
            world.Archetypes.ChangeEntityNameParent(entity, 0, parent);

        return entity;
    }

    public static bool IsChildOf(this Entity entity, Entity potentialParent) =>
        entity.world.IsEntityChildOf(entity, potentialParent);

    public static Entity RemoveChildOf(this Entity entity)
    {
        entity.world.RemoveChildOf(entity);
        return entity;
    }

    public static Entity InstanceOf(this Entity entity, string instanceName)
    {
        entity.world.AddInstanceOf(entity, entity.world.GetEntity(instanceName));
        return entity;
    }

    public static Entity InstanceOf(this Entity entity, Entity instance)
    {
        entity.world.AddInstanceOf(entity, instance);
        return entity;
    }

    public static Entity Add(this Entity target, Entity tag)
    {
        target.world.AddTag(target, tag);
        return target;
    }

    public static Entity Get<T>(this Entity entity, out T component) where T : struct
    {
        component = entity.world.GetComponent<T>(entity).Value;
        return entity;
    }

    public static Entity Get<T>(this Entity entity, Entity target, out T component) where T : struct
    {
        component = entity.world.GetRelationship<T>(entity, target).Value;
        return entity;
    }

    public static EntityWithComponent<T> Get<T>(this Entity entity) where T : struct =>
        entity.world.GetComponent<T>(entity);

    public static EntityWithComponent<T> Get<T>(this Entity entity, Entity target) where T : struct => entity.world.GetRelationship<T>(entity, target);

    public static Entity Set<T>(this Entity entity, T value) where T : struct
    {
        entity.world.GetComponent<T>(entity).Value = value;
        return entity;
    }

    public static Entity Set<T>(this Entity entity, Entity target, T value) where T : struct
    {
        entity.world.GetRelationship<T>(entity, target).Value = value;
        return entity;
    }

    public static bool Has<T>(this Entity entity) where T : struct =>
        entity.world.HasComponent<T>(entity);

    public static bool Has(this Entity entity, Entity tag) =>
         entity.world.HasTag(entity, tag);

    public static Entity Remove(this Entity entity)
    {
        entity.world.RemoveEntity(entity);
        return entity;
    }

    public static Entity Remove<T>(this Entity entity) where T : struct
    {
        entity.world.RemoveComponent<T>(entity);
        return entity;
    }

    public static bool IsAlive(this Entity entity)
    {
        var world = entity.world;
        if (world == null)
            return false;

        return world.IsAlive(entity);
    }

    public static Entity Add(this Entity entity, Entity relation, Entity target)
    {
        entity.world.AddRelationship(entity, relation, target);
        return entity;
    }

    public static bool Has(this Entity entity, Entity relation, Entity target) =>
        entity.world.HasRelationship(entity, relation, target);

    public static void Remove(this Entity entity, Entity relation, Entity target) =>
        entity.world.RemoveRelationship(entity, relation, target);

    public static Entity Add<T>(this Entity entity, Entity target, T value = default) where T : struct
    {
        entity.world.AddRelationship(entity, target, value);
        return entity;
    }

    public static Entity Add<T1, T2>(this Entity entity, T1 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    public static Entity Add<T1, T2>(this Entity entity, T2 value) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    public static Entity Add1<T1, T2>(this Entity entity, T1 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    public static Entity Add2<T1, T2>(this Entity entity, T2 value = default) where T1 : struct where T2 : struct
    {
        entity.world.AddRelationship<T1, T2>(entity, value);
        return entity;
    }

    public static Relationship Find<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.FindRelationship<T>(entity, target);

    public static Relationship Find<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.FindRelationship<T1, T2>(entity);

    public static Entity Set1<T1, T2>(this Entity entity, T1 value) where T1 : struct where T2 : struct
    {
        entity.world.GetRelationship1<T1, T2>(entity).Value = value;
        return entity;
    }

    public static Entity Set2<T1, T2>(this Entity entity, T2 value) where T1 : struct where T2 : struct
    {
        entity.world.GetRelationship2<T1, T2>(entity).Value = value;
        return entity;
    }

    public static Entity Get1<T1, T2>(this Entity entity, out T1 component) where T1 : struct where T2 : struct
    {
        component = entity.world.GetRelationship1<T1, T2>(entity).Value;
        return entity;
    }

    public static Entity Get2<T1, T2>(this Entity entity, out T2 component) where T1 : struct where T2 : struct
    {
        component = entity.world.GetRelationship2<T1, T2>(entity).Value;
        return entity;
    }

    public static EntityWithComponent<T1> Get1<T1, T2>(this Entity entity) where T1 : struct where T2 : struct => entity.world.GetRelationship1<T1, T2>(entity);

    public static EntityWithComponent<T2> Get2<T1, T2>(this Entity entity) where T1 : struct where T2 : struct => entity.world.GetRelationship2<T1, T2>(entity);

    public static bool Has<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.HasRelationship<T1, T2>(entity);

    public static void Remove<T1, T2>(this Entity entity) where T1 : struct where T2 : struct =>
        entity.world.RemoveRelationship<T1, T2>(entity);

    public static bool Has<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.HasRelationship<T>(entity, target);

    public static void Remove<T>(this Entity entity, Entity target) where T : struct =>
        entity.world.RemoveRelationship<T>(entity, target);

    public static RelationshipEnumeratorGetter GetRelationships(this Entity entity) =>
        entity.world.GetRelationships(entity);

    public static ComponentEnumeratorGetter GetComponents(this Entity entity) =>
        entity.world.GetComponents(entity);

    public static bool IsIndexEqual(Entity entity, Entity otherEntity) =>
        IdConverter.GetFirst(entity) == otherEntity;

    public static Filter GetChildren(this Entity enitity) => enitity.world.GetChildren(enitity);
}