using System.Runtime.CompilerServices;

namespace ECS;

/// <summary>
/// Shouldn't be used anywhere but when calling GetRelationships(), GetRelationship() or FindRelationship()
/// </summary>
public readonly struct Relationship
{
    public readonly ECSWorld world;
    public readonly ulong value;

    public Relationship(ECSWorld world, ulong value)
    {
        this.world = world;
        this.value = value;
    }

    public static implicit operator ulong(Relationship relationship) => relationship.value;

    public static implicit operator Relationship(ulong relationship) => new Relationship(null!, relationship);
}

public struct RelationshipEnumerator
{
    private readonly Archetype _archetype;
    private readonly ulong[] _components;
    private int _lastIndex;

    public RelationshipEnumerator(Archetype archetype)
    {
        _archetype = archetype;
        _components = _archetype.componentsArray;
    }

    public bool MoveNext()
    {
        if (_components == null)
            return false;

        while (true)
        {
            if (_lastIndex == _components.Length)
                break;

            var current = _components[_lastIndex++];
            if (!IdConverter.IsRelationship(current))
                continue;

            Current = current;
            return true;
        }

        return false;
    }

    public Relationship Current { get; private set; }
}

public struct ComponentEnumerator
{
    private readonly Archetype _archetype;
    private readonly ulong[] _components;
    private int _lastIndex;

    public ComponentEnumerator(Archetype archetype)
    {
        _archetype = archetype;
        _components = _archetype.componentsArray;
    }

    public bool MoveNext()
    {
        if (_components == null || _lastIndex >= _components.Length)
            return false;

        Current = _components[_lastIndex++];
        return true;
    }

    public ulong Current { get; private set; }
}

public sealed class ECSWorld
{
    public Archetypes Archetypes => _archetypes;
    internal static ECSWorld? Instance { get; private set; }

    private readonly Archetypes _archetypes;
    private readonly IntPtr[] _singeItemArray = new IntPtr[1];
    private readonly List<ECSSystems> _ecsSystems;

    public ECSWorld()
    {
        if (Instance != null)
            throw new Exception($"More than one {nameof(ECSWorld)} is not supported");

        Instance = this;
        _archetypes = new(this);

        _ecsSystems = new();
    }

    internal void AddSystems(ECSSystems systems)
    {
        _ecsSystems.Add(systems);
        _archetypes.AddOnActionComponentsSystems(systems);
    }


    public Entity CopyEntity(Entity entityToCopy)
    {
        var copyEntity = AddEntity();
        _archetypes.CopyEntityComponents(AddEntity(), entityToCopy);
        return copyEntity;
    }

    public Filter GetChildren(Entity entity) =>
        _archetypes.GetChildren(entity);

    public bool IsEntityChildOf(Entity entity, Entity potentialParent) =>
        HasRelationship<ChildOf>(entity, potentialParent);

    public void AddChildOf(Entity entity, Entity parent) =>
        AddRelationship<ChildOf>(entity, parent);

    public void RemoveChildOf(Entity entity)
    {
        var relationship = entity.Find<ChildOf, Wildcard>();
        RemoveRelationship<ChildOf>(entity, relationship.GetTarget());
    }

    public void SetPrefabValue<T>(Entity prefabEntity, T value, ModifyPrefabValue<T> modifyPrefabValuePrefab = null!) where T : struct
    {
#if DEBUG
        if (!HasComponent<Prefab>(prefabEntity))
            throw new Exception($"{prefabEntity} is not a prefab");
#endif

        _archetypes.SetPrefabValue(prefabEntity, value, modifyPrefabValuePrefab);
    }

    public void SetPrefabValue<T1, T2>(Entity prefabEntity, T1 value, ModifyPrefabValue<T1> modifyPrefabValuePrefab = null!) where T1 : struct where T2 : struct
    {
#if DEBUG
        if (!HasComponent<Prefab>(prefabEntity))
            throw new Exception($"{prefabEntity} is not a prefab");
#endif

        _archetypes.SetPrefabValue(prefabEntity, value, modifyPrefabValuePrefab);
    }

    public void SetPrefabValue<T1, T2>(Entity prefabEntity, T2 value, ModifyPrefabValue<T2> modifyPrefabValuePrefab = null!) where T1 : struct where T2 : struct
    {
#if DEBUG
        if (!HasComponent<Prefab>(prefabEntity))
            throw new Exception($"{prefabEntity} is not a prefab");
#endif

        _archetypes.SetPrefabValue(prefabEntity, value, modifyPrefabValuePrefab);
    }

    public void SetPrefabValue<T>(Entity target, Entity prefabEntity, T value, ModifyPrefabValue<T> modifyPrefabValuePrefab = null!) where T : struct
    {
#if DEBUG
        if (!HasComponent<Prefab>(prefabEntity))
            throw new Exception($"{prefabEntity} is not a prefab");
#endif

        _archetypes.SetPrefabValue(target, prefabEntity, value, modifyPrefabValuePrefab);
    }

    public ulong GetEntityFromIndex(uint index)
    {
        ref var record = ref _archetypes.GetEntityRecord(index);
        return record.entity;
    }

    public ulong GetRelationshipRelation(ulong relationship)
    {
        var first = IdConverter.GetFirst(relationship);
        if (first == Archetypes.wildCard32)
            return IdConverter.Compose(first, 0, false);

        return GetEntityFromIndex(first);
    }

    public ulong GetRelationshipTarget(ulong relationship)
    {
        var second = IdConverter.GetSecond(relationship);
        if (second == Archetypes.wildCard31)
            return IdConverter.Compose(Archetypes.wildCard32, 0, false);

        return GetEntityFromIndex(second);
    }

    public Relationship GetRelationship<T1, T2>() where T1 : struct where T2 : struct =>
        new(this, _archetypes.GetRelationship(IndexOf<T1>(), IndexOf<T2>()));

    public Relationship GetRelationship<T>(Entity target) where T : struct =>
        new(this, _archetypes.GetRelationship(IndexOf<T>(), target));

    public Relationship GetRelationship(Entity relation, Entity target) =>
        new(this, _archetypes.GetRelationship(relation, target));

    public Relationship FindRelationship<T>(Entity entity, Entity target) where T : struct
    {
        var relationshipToFind = GetRelationship<T>(target);
        var targetToFind = relationshipToFind.GetTarget();
        var targetIndex = IdConverter.GetFirst(targetToFind);
        var relationToFind = relationshipToFind.GetRelation();
        var relationIndex = IdConverter.GetFirst(relationToFind);

        if (relationshipToFind == _archetypes.wildCardRelationship)
        {
            var enumerator = GetRelationships(entity).GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            return 0;
        }
        else if (relationIndex == Archetypes.wildCard32)
        {
            foreach (var relationship in entity.GetRelationships())
            {
                if (GetRelationshipTarget(relationship) != targetToFind)
                    continue;

                return new(this, relationship);
            }
        }
        else if (targetIndex == Archetypes.wildCard32)
        {
            foreach (var relationship in entity.GetRelationships())
            {
                if (GetRelationshipRelation(relationship) != relationToFind)
                    continue;

                return new(this, relationship);
            }
        }
        else
        {
            foreach (var relationship in entity.GetRelationships())
            {
                if (relationship == relationshipToFind)
                    return new(this, relationship);
            }
        }

        return 0;
    }

    public Relationship FindRelationship<T1, T2>(Entity entity) where T1 : struct where T2 : struct
    {
        var relationshipToFind = GetRelationship<T1, T2>();
        var targetToFind = relationshipToFind.GetTarget();
        var targetIndex = IdConverter.GetFirst(targetToFind);
        var relationToFind = relationshipToFind.GetRelation();
        var relationIndex = IdConverter.GetFirst(relationToFind);

        if (relationshipToFind == _archetypes.wildCardRelationship)
        {
            var enumerator = GetRelationships(entity).GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            return 0;
        }
        else if (relationIndex == Archetypes.wildCard32)
        {
            foreach (var relationship in GetRelationships(entity))
            {
                if (GetRelationshipTarget(relationship) != targetToFind)
                    continue;

                return new(this, relationship);
            }
        }
        else if (targetIndex == Archetypes.wildCard32)
        {
            foreach (var relationship in GetRelationships(entity))
            {
                if (GetRelationshipRelation(relationship) != relationToFind)
                    continue;

                return new(this, relationship);
            }
        }
        else
        {
            foreach (var relationship in GetRelationships(entity))
            {
                if (relationship == relationshipToFind)
                    return new(this, relationship);
            }
        }

        return 0;
    }

    public bool RelationshipHasTarget<T>(ulong relationship) where T : struct =>
        IdConverter.GetFirst(relationship) == IdConverter.GetFirst(IndexOf<T>());

    public bool RelationshipHasTarget(ulong relationship, ulong target) =>
        IdConverter.GetFirst(relationship) == IdConverter.GetFirst(target);

    public bool RelationshipHasRelation<T>(ulong relationship) where T : struct =>
        IdConverter.GetSecond(relationship) == IdConverter.GetFirst(IndexOf<T>());

    public bool RelationshipHasRelation(ulong relationship, ulong relation) =>
        IdConverter.GetSecond(relationship) == IdConverter.GetFirst(relation);

    public bool RelationshipIs<T1, T2>(ulong relationship) where T1 : struct where T2 : struct =>
        relationship == IdConverter.Compose(IdConverter.GetFirst(IndexOf<T1>()), IdConverter.GetFirst(IndexOf<T2>()), true);

    public bool RelationshipIs<T>(ulong relationship, ulong target) where T : struct =>
        relationship == IdConverter.Compose(IdConverter.GetFirst(IndexOf<T>()), IdConverter.GetFirst(target), true);

    public bool RelationshipIs(ulong relationship, ulong relation, ulong target) =>
      relationship == IdConverter.Compose(IdConverter.GetFirst(target), IdConverter.GetFirst(target), true);

    public Entity AddEntity(string? name = default)
    {
        var entity = _archetypes.AddEntity();
        if (name != default && name != string.Empty)
            _archetypes.AddEntityName(entity, 0, name);

        return entity;
    }

    public Entity AddPrefab(string? name = default)
    {
        var entity = AddEntity(name);
        entity.Add<Prefab>();

        return entity;
    }

    public void AddInstanceOf(Entity entity, Entity instace) =>
        _archetypes.AddInstanceOf(entity, instace);

    public void RemoveEntity(Entity entity) => _archetypes.RemoveEntity(entity);

    public bool IsAlive(Entity entity) => _archetypes.IsEntityAlive(entity);

    public void AddComponent<T>(Entity entity, T value) where T : struct =>
         _archetypes.AddComponent(IndexOf<T>(), entity, value);

    public ref T GetComponent<T>(Entity entity) where T : struct =>
        ref _archetypes.GetComponent<T>(IndexOf<T>(), entity);

    public bool HasComponent<T>(Entity entity) where T : struct =>
        _archetypes.HasComponent(IndexOf<T>(), entity);

    public bool HasTag(Entity entity, Entity tag) =>
        _archetypes.HasComponent(tag, entity);

    public bool RemoveComponent<T>(Entity entity) where T : struct =>
        _archetypes.RemoveComponent<T>(IndexOf<T>(), entity);

    public bool HasName(Entity entity)
    {
        var parent = IdConverter.GetSecond(FindRelationship<ChildOf, Wildcard>(entity));
        return _archetypes.EntityHasName(entity, parent);
    }

    public void Name(Entity entity, string name)
    {
        var parent = IdConverter.GetSecond(FindRelationship<ChildOf, Wildcard>(entity));

        if (_archetypes.EntityHasName(entity, parent))
        {
            _archetypes.ChangeEntityName(entity, parent, name);
            return;
        }

        _archetypes.AddEntityName(entity, parent, name);
    }

    public void RemoveName(Entity entity)
    {
        if (!entity.Has<ChildOf, Wildcard>())
        {
            _archetypes.RemoveEntityName(entity, 0);
            return;
        }

        var chilOfIndex = _archetypes.GetComponentIndex<ChildOf>();

        foreach (var relationship in entity.GetRelationships())
        {
            if (IdConverter.GetFirst(relationship) != chilOfIndex)
                continue;

            var target = GetRelationshipTarget(relationship);
            _archetypes.RemoveEntityName(entity, target);
            return;
        }
    }

    public Entity GetEntity(string name, Entity parent = default) => new(_archetypes.GetEntityByName(name, parent), this);

    public string GetName(Entity entity)
    {
        if (!entity.Has<ChildOf, Wildcard>())
            return _archetypes.GetNameByEntity(entity, 0);

        var chilOfIndex = IdConverter.GetFirst(_archetypes.GetComponentIndex<ChildOf>());

        foreach (var relationship in entity.GetRelationships())
        {
            if (IdConverter.GetFirst(relationship) != chilOfIndex)
                continue;

            var target = GetRelationshipTarget(relationship);
            return _archetypes.GetNameByEntity(entity, target);
        }

        return string.Empty;
    }

    public string GetName(Entity entity, Entity parent) =>
        _archetypes.GetNameByEntity(entity, parent);

    public void AddSingletonEvent<T>(T value = default) where T : struct, ISingletonEvent
    {
        var index = IndexOf<T>();

        if (HasComponent<T>(index))
        {
            _archetypes.GetComponent<T>(index, index) = value;
            return;
        }

        _archetypes.AddComponent(index, index, value, Archetypes.singletonComponentCapacity);
    }

    public bool WasSingletonCreated<T>() where T : struct, ISingletonEvent
    {
        var index = IndexOf<T>();
        return HasComponent<T>(index);
    }

    public ref T GetSingletonEvent<T>() where T : struct, ISingletonEvent
    {
        var index = IndexOf<T>();

        if (!HasComponent<T>(index))
            AddSingletonEvent<T>();

        return ref _archetypes.GetComponent<T>(index, index);
    }

    public void RemoveSingletonEvent<T>() where T : struct, ISingletonEvent
    {
        var index = IndexOf<T>();

        if (!HasComponent<T>(index))
#if DEBUG
            throw new Exception("Singleton event hasn't been created yet.");
#else
            return;
#endif

        _archetypes.RemoveComponent<T>(index, index);
    }

    public void AddEvent<T>(T value = default) where T : struct, IEvent
    {
        var index = IndexOf<T>();

        if (Unsafe.SizeOf<T>() == 1)
        {
            _archetypes.AddTagEvent<T>(index, AddEntity());
            return;
        }

        _archetypes.AddEvent(index, AddEntity(), value);
    }

    /// <summary>
    /// Covers Component-Entity and TagComponent-Entity relations
    /// </summary>
    public void AddRelationship<T>(Entity entity, Entity target, T value = default) where T : struct
    {
        var relation = IndexOf<T>();
        var relationship = _archetypes.GetRelationship(relation, target);

        if (Unsafe.SizeOf<T>() == 1)
        {
            AddRelationship(entity, relation, target);
            return;
        }

        var oldArchetype = _archetypes.GetArchetype(entity);
        if (oldArchetype.GetTableEdge(relationship).Add == null)
            GetComponent<Component>(relation).size = Unsafe.SizeOf<T>();

        _archetypes.AddDataRelationship<T>(entity, relation, target) = value;
    }

    public ref T GetRelationship<T>(Entity entity, ulong relationship) where T : struct
    {
        var relation = IndexOf<T>();
        var target = new Entity(IdConverter.Compose(IdConverter.GetSecond(relationship), 0, false), this);

        ref var record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetype(entity);
        var storage = archetype.GetStorage<T>(_archetypes.GetRelationship(relation, target));
        return ref storage[record.tableRow];
    }

    public ref T GetRelationship<T>(Entity entity, Entity target) where T : struct
    {
        if (Unsafe.SizeOf<T>() == 1)
            return ref Unsafe.As<T[]>(_singeItemArray)[0];

        var relation = IndexOf<T>();

        ref var record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetype(entity);
        var storage = archetype.GetStorage<T>(_archetypes.GetRelationship(relation, target));
        return ref storage[record.tableRow];
    }

    public bool HasRelationship<T>(Entity entity, Entity target) where T : struct
    {
        var relation = IndexOf<T>();
        return _archetypes.HasRelationship(entity, relation, target);
    }

    public void RemoveRelationship<T>(Entity entity, Entity target) where T : struct
    {
        var relation = IndexOf<T>();

        if (Unsafe.SizeOf<T>() == 1)
        {
            RemoveRelationship(entity, relation, target);
            return;
        }

        _archetypes.RemoveDataRelationship(entity, relation, target);
    }

    /// <summary>
    /// Covers TagComponent-TagComponent, Component-TagComponent and TagComponent-Component
    /// </summary>
    public void AddRelationship<T1, T2>(Entity entity, T1 value) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();
        var relationship = _archetypes.GetRelationship(relation, target);

        if (Unsafe.SizeOf<T1>() == 1 && Unsafe.SizeOf<T2>() == 1)
        {
            _archetypes.AddRelationship(entity, relation, target);
            return;
        }

        var oldArchetype = _archetypes.GetArchetype(entity);
        if (oldArchetype.GetTableEdge(relationship).Add == null)
            GetComponent<Component>(relation).size = Unsafe.SizeOf<T1>();

        _archetypes.AddDataRelationship<T1>(entity, relation, target) = value;
    }

    public void AddRelationship<T1, T2>(Entity entity, T2 value) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();
        var relationship = _archetypes.GetRelationship(relation, target);

        if (Unsafe.SizeOf<T1>() == 1 && Unsafe.SizeOf<T2>() == 1)
        {
            _archetypes.AddRelationship(entity, relation, target);
            return;
        }

        var oldArchetype = _archetypes.GetArchetype(entity);
        if (oldArchetype.GetTableEdge(relationship).Add == null)
            GetComponent<Component>(target).size = Unsafe.SizeOf<T2>();

        _archetypes.AddDataRelationship<T2>(entity, relation, target) = value;
    }

    public ref T1 GetRelationship1<T1, T2>(Entity entity) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();

        if (Unsafe.SizeOf<T1>() == 1)
            return ref Unsafe.As<T1[]>(_singeItemArray)[0];

        ref var record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetype(entity);
        var storage = archetype.GetStorage<T1>(_archetypes.GetRelationship(relation, target));
        return ref storage[record.tableRow];
    }

    public ref T2 GetRelationship2<T1, T2>(Entity entity) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();

        if (Unsafe.SizeOf<T2>() == 1)
            return ref Unsafe.As<T2[]>(_singeItemArray)[0];

        ref var record = ref _archetypes.GetEntityRecord(entity);
        var archetype = _archetypes.GetArchetype(entity);
        var storage = archetype.GetStorage<T2>(_archetypes.GetRelationship(relation, target));
        return ref storage[record.tableRow];
    }

    public bool HasRelationship<T1, T2>(Entity entity) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();
        return _archetypes.HasRelationship(entity, relation, target);
    }

    public void RemoveRelationship<T1, T2>(Entity entity) where T1 : struct where T2 : struct
    {
        var relation = IndexOf<T1>();
        var target = IndexOf<T2>();

        if (Unsafe.SizeOf<T1>() == 1 && Unsafe.SizeOf<T2>() == 1)
        {
            RemoveRelationship(entity, relation, target);
            return;
        }

        _archetypes.RemoveDataRelationship(entity, relation, target);
    }

    /// <summary>
    /// Covers Entity-Entity
    /// </summary>
    public void AddRelationship(Entity entity, Entity relation, Entity target)
    {
        _archetypes.AddRelationship(entity, relation, target);
    }

    public bool HasRelationship(Entity entity, Entity relation, Entity target)
    {
        return _archetypes.HasRelationship(entity, relation, target);
    }

    public void RemoveRelationship(Entity entity, Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);
        var relationComponent = IdConverter.Compose(relationId, targetId, true);

        _archetypes.RemoveComponent(relationComponent, entity, out _, out _, true);
    }

    public Entity GetRelationEntity(ulong relationship) =>
         new(IdConverter.Compose(IdConverter.GetFirst(relationship), 0, false), this);


    public Entity GetTargetEntity(ulong relationship) =>
         new(IdConverter.Compose(IdConverter.GetSecond(relationship), 0, false), this);


    public RelationshipEnumeratorGetter GetRelationships(Entity entity) =>
         new(GetArchetype(entity));

    public ComponentEnumeratorGetter GetComponents(Entity entity) =>
        new(GetArchetype(entity));

    public void AddTag(Entity target, Entity tag) =>
        _archetypes.AddEntityTag(target, tag);

    public void AddTag<T>(Entity entity) where T : struct
    {
        var tag = IndexOf<T>();
        _archetypes.AddEntityTag(entity, tag);
    }

    public Archetype GetArchetype(Entity entity) => _archetypes.GetArchetype(entity);

    public EnumeratorSingleGetter<T> GetEvents<T>() where T : struct, IEvent =>
        _archetypes.GetEvents<T>();

    public void RemoveEvents<T>() where T : struct, IEvent =>
        _archetypes.RemoveEvents<T>(IndexOf<T>());

    public void Destroy()
    {
        foreach (var systems in _ecsSystems)
            systems.Destroy();
    }

    public FilterBuilder Filter()
    {
        return new FilterBuilder(_archetypes);
    }

    public FilterBuilder<C> Filter<C>()
        where C : struct
    {
        return new FilterBuilder<C>(_archetypes);
    }

    public FilterBuilder<C1, C2> Filter<C1, C2>()
        where C1 : struct
        where C2 : struct
    {
        return new FilterBuilder<C1, C2>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3> Filter<C1, C2, C3>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
    {
        return new FilterBuilder<C1, C2, C3>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3, C4> Filter<C1, C2, C3, C4>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
    {
        return new FilterBuilder<C1, C2, C3, C4>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3, C4, C5> Filter<C1, C2, C3, C4, C5>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
    {
        return new FilterBuilder<C1, C2, C3, C4, C5>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6> Filter<C1, C2, C3, C4, C5, C6>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
    {
        return new FilterBuilder<C1, C2, C3, C4, C5, C6>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7> Filter<C1, C2, C3, C4, C5, C6, C7>()
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
    {
        return new FilterBuilder<C1, C2, C3, C4, C5, C6, C7>(_archetypes);
    }

    public FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Filter<C1, C2, C3, C4, C5, C6, C7, C8>()
       where C1 : struct
       where C2 : struct
       where C3 : struct
       where C4 : struct
       where C5 : struct
       where C6 : struct
       where C7 : struct
       where C8 : struct
    {
        return new FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8>(_archetypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong IndexOf<T>() where T : struct => _archetypes.GetComponentIndex<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type TypeOf(ulong component) => TypeData.TypesByIndices[component];
}