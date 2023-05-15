using System.Runtime.CompilerServices;

namespace ECS;

public delegate void ModifyPrefabValue<T>(ref T component, T value) where T : struct;

public struct AdditionalEntityData
{
    private byte _data;

    public void Set(byte type, bool value) => _data = (byte)(value ? _data | (1 << type) : _data & 1);

    public bool Get(byte type) => (_data & (1 << type)) > 0;

    public void Clear() => _data = 0;
}

public struct EntityRecord
{
    public int archetypeRow;
    public int tableRow;
    public int archetypeId;
    public ulong entity;
    public AdditionalEntityData additionalData;

    public bool IsTag() => additionalData.Get(0);

    public void SetIsTag(bool value) => additionalData.Set(0, value);

    public bool IsRelation() => additionalData.Get(1);

    public void SetIsRelation(bool value) => additionalData.Set(1, value);

    public bool IsTarget() => additionalData.Get(2);

    public void SetIsTarget(bool value) => additionalData.Set(2, value);

    public bool HasRelationships() => additionalData.Get(3);

    public void SetHasRelationsips(bool value) => additionalData.Set(3, value);
}

internal struct OnComponentSystems
{
    public List<OnComponentActionSystem> systems;
    public bool systemsExist;
}

internal enum ArchetypeOperationType
{
    AddComponent,
    RemoveComponent,
    RemoveEntity,
}

internal readonly struct ArchetypeOperation
{
    public readonly ArchetypeOperationType operationType;
    public readonly ulong type;
    public readonly ulong entity;
    public readonly int index;

    public ArchetypeOperation(ArchetypeOperationType operationType, ulong type, ulong entity, int index)
    {
        this.operationType = operationType;
        this.type = type;
        this.entity = entity;
        this.index = index;
    }
}

internal struct ArchetypeOperationStorage
{
    public Array array;
    public int count;

    private Type _elementType;

    public ArchetypeOperationStorage(Array array, Type elementType)
    {
        this.array = array;
        _elementType = elementType;
    }

    public void Clear()
    {
        count = 0;
    }

    public ref T Get<T>(int index) where T : struct
    {
        return ref ((T[])array)[index];
    }

    public ref T Add<T>(out int index) where T : struct
    {
        EnsureCapacity(count + 1);

        index = count;

        ref T value = ref ((T[])array)[count++];
        value = default;

        return ref value;
    }

    public void Add(out int index)
    {
        EnsureCapacity(count + 1);

        index = count;
        count++;
    }

    private void EnsureCapacity(int capacity)
    {
        if (capacity <= array.Length)
            return;

        var newCapacity = (capacity - 1) << 1;

        var newStorage = Array.CreateInstance(_elementType, newCapacity);
        Array.Copy(array, newStorage, capacity - 1);
        array = newStorage;
    }
}

public enum AutoResetState
{
    OnAdd,
    OnRemove
}

public interface IAutoReset<T> where T : struct
{
    void AutoReset(ref T component, AutoResetState state);
}

internal delegate void AutoReset<T>(ref T component, AutoResetState state) where T : struct;

internal static class AutoResets<T> where T : struct
{
    public static AutoReset<T> Handler { get; set; } = null!;
}

public struct Component
{
    public Type type;
    public int size;
}

public struct EntityAndParent
{
    public ulong entity;
    public ulong parentEntity;

    public EntityAndParent(ulong entity, ulong parentEntity)
    {
        this.entity = entity;
        this.parentEntity = parentEntity;
    }
}

public struct NameAndParent
{
    public string name;
    public ulong parentEntity;

    public NameAndParent(string name, ulong parentEntity)
    {
        this.name = name;
        this.parentEntity = parentEntity;
    }
}

public struct IsA { }
public struct Prefab { }
public struct ChildOf { }
public struct Wildcard { }

public sealed class Archetypes
{
    internal ECSWorld World => _world;
    internal Archetype EntityArchetype => ((Archetype?)_archetypes[0]!.Target)!;

    public const uint wildCard32 = uint.MaxValue;
    // since gen/entity is only 31 bits, it needs separate wildCard Instance
    public const uint wildCard31 = uint.MaxValue >> 1;
    public const ulong componentType = 1;
    public const int singletonComponentCapacity = 1;
    public const int componentsCapacity = 512;
    public const int relationshipsCapacity = 4;
    public const ulong entityType = 0;
    public readonly ulong wildCardRelationship;

    private const int _talbeOperationComponentsCapacity = 64;
    private const ulong _nullEntity = 0;
    private readonly Queue<ulong> _unusedIds;
    //as long as filters are stored somewhere in users code, they won't be GC'ed
    private readonly Dictionary<int, List<WeakReference>> _filtersByHash;
    private readonly Dictionary<ulong, List<Filter>> _filtersWithTagsByTypes;
    private readonly Dictionary<NameAndParent, ulong> _entitesByNames;
    private readonly Dictionary<EntityAndParent, string> _namesByEntities;
    private readonly Dictionary<ulong, List<WeakReference>> _archetypesByHashes;
    private readonly Dictionary<ulong, List<WeakReference>> _tablesByHash;
    private readonly Dictionary<ulong, ArchetypeOperationStorage> _archetypeOperationStorages;
    private readonly Dictionary<ulong, HashSet<Archetype>> _archetypesByTypes;
    private readonly Dictionary<ulong, Filter> _childrenFilters;
    private readonly List<WeakReference> _archetypes;
    private readonly List<ArchetypeOperation> _archetypeOperations;
    private readonly List<OnComponentActionSystem> _onComponentSystems;
    private readonly Dictionary<int, List<OnComponentActionSystem>> _onComponentSystemsByArchetypeIds;
    private EntityRecord[] _entityRecords;
    private readonly ECSWorld _world;
    private uint _entityCount = 1;
    private bool _locked;
    private int _lockedCount;

    public Archetypes(ECSWorld world)
    {
        _world = world;
        _archetypes = new();
        _entityRecords = new EntityRecord[componentsCapacity];
        Array.Fill(_entityRecords, new() { archetypeId = -1, archetypeRow = -1, tableRow = -1 });
        _unusedIds = new();
        _filtersByHash = new();
        _filtersWithTagsByTypes = new();
        _entitesByNames = new();
        _namesByEntities = new();
        _onComponentSystems = new();
        _childrenFilters = new();
        _archetypeOperationStorages = new();
        _archetypeOperations = new();
        _tablesByHash = new();
        _archetypesByTypes = new();
        _archetypesByHashes = new();
        _onComponentSystemsByArchetypeIds = new();
        wildCardRelationship = IdConverter.Compose(wildCard32, wildCard32, true);

        TypeData.AddTypeAndIndex(typeof(Entity), entityType);
        var types = new SortedSet<ulong>() { entityType };
        AddArchetype(new Table(this, types, componentsCapacity, relationshipsCapacity), types);

        TypeData.AddTypeAndIndex(typeof(Component), componentType);
        TypeData<Component>.index = componentType;
        TypeData.AddTypeAndIndex(typeof(Wildcard), IdConverter.Compose(uint.MaxValue, uint.MaxValue, false));
    }

    internal void AddOnActionComponentsSystems(ECSSystems systems)
    {
        var allSystems = systems.Systems;

        foreach (var system in allSystems)
        {
            if (system is GroupSystem groupSystem)
            {
                AddOnComponentSystems(groupSystem.Systems);
            }
        }

        AddOnComponentSystems(systems.Systems);
    }

    internal void Lock()
    {
        _lockedCount++;
        _locked = true;
    }

    internal void Unlock()
    {
        _lockedCount--;
        if (_lockedCount != 0)
            return;

        _locked = false;

        Archetype? oldArchetype;

        for (int i = 0; i < _archetypeOperations.Count; i++)
        {
            var operation = _archetypeOperations[i];
            if (!IsEntityAlive(operation.entity))
                continue;

            switch (operation.operationType)
            {
                case ArchetypeOperationType.RemoveComponent:
                    if (!HasComponent(operation.type, operation.entity))
                        continue;

                    oldArchetype = GetArchetype(operation.entity);
                    RemoveComponent(operation.type, operation.entity, out _, out _, operation.index == -1);

                    if (operation.type != componentType)
                        TryCallOnComponentRemoveSystems(operation.entity, oldArchetype, operation.type);
                    break;
                case ArchetypeOperationType.AddComponent:
                    if (!HasComponent(operation.type, operation.entity))
                    {
                        AddComponent(operation.type, operation.entity, out var archetype, out _, operation.index == -1);
                        if (operation.type != componentType)
                            TryCallOnComponentAddSystems(operation.entity, archetype, operation.type);
                    }

                    if (operation.index == -1)
                        continue;

                    ref var record = ref GetEntityRecord(operation.entity);
                    oldArchetype = (Archetype?)_archetypes[record.archetypeId].Target!;
                    var storage = _archetypeOperationStorages[operation.type];
                    Array.Copy(storage.array, operation.index, oldArchetype.GetStorage(operation.type), record.tableRow, 1);

                    break;
                case ArchetypeOperationType.RemoveEntity:
                    RemoveEntity(operation.entity);
                    break;
            }
        }

        _archetypeOperations.Clear();
        foreach (var (_, storage) in _archetypeOperationStorages)
        {
            storage.Clear();
        }
    }

    public void ChangeEntityName(ulong entity, ulong parentEntity, string name)
    {
        var oldName = GetNameByEntity(entity, parentEntity);

#if DEBUG
        if (oldName == string.Empty)
            throw new Exception($"Entity with name {name} and parent {parentEntity} does not exsist");
#endif

        var oldNameData = new NameAndParent(oldName, parentEntity);
        var newNameData = new NameAndParent(name, parentEntity);
        var parentData = new EntityAndParent(entity, parentEntity);

        _entitesByNames.Remove(oldNameData);
        _entitesByNames[newNameData] = entity;
        _namesByEntities[parentData] = name;
    }

    public bool EntityHasName(ulong entiy, ulong parent) =>
        _namesByEntities.ContainsKey(new(entiy, parent));

    public string GetNameByEntity(ulong entity, ulong parent)
    {
        var parentData = new EntityAndParent(entity, parent);

        if (!_namesByEntities.TryGetValue(parentData, out var name))
#if DEBUG
            throw new Exception($"Entity {entity} did not have a name");
#else
            return string.Empty;
#endif

        return name;
    }

    public void ChangeEntityNameParent(ulong entity, ulong oldParent, ulong newParent)
    {
        var oldParentData = new EntityAndParent(entity, oldParent);

        if (!_namesByEntities.TryGetValue(oldParentData, out var name))
            return;

        _namesByEntities.Remove(oldParentData);
        var newParentData = new EntityAndParent(entity, newParent);
        _namesByEntities.Add(newParentData, name);

        _entitesByNames.Remove(new NameAndParent(name, oldParent));
        _entitesByNames.Add(new NameAndParent(name, newParent), entity);
    }

    public ulong GetEntityByName(string name, ulong parent)
    {
        if (!_entitesByNames.TryGetValue(new NameAndParent(name, parent), out var entity))
            return _nullEntity;

        return entity;
    }

    public void RemoveEntityName(ulong entity, ulong parent)
    {
        var parentData = new EntityAndParent(entity, parent);

        if (!_namesByEntities.TryGetValue(parentData, out var name))
            return;

        _namesByEntities.Remove(parentData);
        _entitesByNames.Remove(new NameAndParent(name, parent));
    }

    public void AddEntityName(ulong entity, ulong parent, string name)
    {
        var nameData = new NameAndParent(name, parent);
        var parentData = new EntityAndParent(entity, parent);

        if (!_entitesByNames.TryGetValue(nameData, out var entityByName))
        {
            entityByName = entity;
            _entitesByNames[nameData] = entityByName;
            _namesByEntities[parentData] = name;
            return;
        }

#if DEBUG
        var parentName = GetEntityNameOrValue(parent);
        throw new Exception($"Entity with name {name} and parent {parentName} aldready exists");
#else
        _entitesByNames[nameData] = entity;
        _namesByEntities[parentData] = name;
#endif
    }

    public Archetype GetArchetype(Entity entity) =>
        ((Archetype?)_archetypes[_entityRecords[IdConverter.GetFirst(entity)].archetypeId].Target)!;

    public Entity AddEntity()
    {
        var entityValue = GetNewEntity(false);
        var entity = new Entity(entityValue, _world);
        var archetypeRef = _archetypes[0];
        var archetype = ((Archetype?)archetypeRef.Target)!;
        var archetypeRow = archetype.Add(entityValue, out int tableRow);
        var id = IdConverter.GetFirst(entityValue);

        if (_entityRecords.Length == _entityCount)
        {
            Array.Resize(ref _entityRecords, (int)(_entityCount << 1));
            Array.Fill(_entityRecords, new() { archetypeId = -1, archetypeRow = -1, tableRow = -1 }, (int)_entityCount, _entityRecords.Length - (int)_entityCount);
        }

        _entityRecords[id] = new EntityRecord() { archetypeRow = archetypeRow, tableRow = tableRow, archetypeId = archetype.id, entity = entityValue };

        ((Entity[])archetype.Storages[0])[archetypeRow] = entity;

        return entity;
    }

    public void AddInstanceOf(Entity entity, Entity instance)
    {
        CopyEntityComponents(entity, instance, true);
        entity.Add<IsA>(instance);
    }

    public void SetPrefabValue<T>(Entity prefabEntity, T value, ModifyPrefabValue<T>? modifyPrefabValueDelegate = null) where T : struct
    {
        var relationship = _world.GetRelationship<IsA>(prefabEntity);
        var componentIndex = GetComponentIndex<T>();

        if (!_archetypesByTypes.TryGetValue(relationship, out var archetypes))
            return;

        foreach (var archetype in archetypes)
        {
            for (int i = 0; i < archetype.Count; i++)
            {
                var entity = archetype.Entities[i];
                ref var component = ref GetComponent<T>(componentIndex, entity);

                if (modifyPrefabValueDelegate != null)
                {
                    modifyPrefabValueDelegate(ref component, value);
                    continue;
                }

                component = value;
            }
        }
    }

    public void SetPrefabValue<T1, T2>(Entity prefabEntity, T1 value, ModifyPrefabValue<T1>? modifyPrefabValueDelegate = null) where T1 : struct where T2 : struct
    {
        var isARelationship = _world.GetRelationship<IsA>(prefabEntity);
        var relation = GetComponentIndex<T1>();
        var target = GetComponentIndex<T2>();
        var relationship = GetRelationship(relation, target);

        if (!_archetypesByTypes.TryGetValue(isARelationship, out var archetypes))
            return;

        foreach (var archetype in archetypes)
        {
            var storage = archetype.GetStorage<T1>(relationship);

            for (int i = 0; i < archetype.Count; i++)
            {
                var entity = archetype.Entities[i];

                ref var record = ref GetEntityRecord(entity);
                ref var component = ref storage[record.tableRow];

                if (modifyPrefabValueDelegate != null)
                {
                    modifyPrefabValueDelegate(ref component, value);
                    continue;
                }

                component = value;
            }
        }
    }

    public void SetPrefabValue<T>(Entity target, Entity prefabEntity, T value, ModifyPrefabValue<T>? modifyPrefabValueDelegate = null) where T : struct
    {
        var isARelationship = _world.GetRelationship<IsA>(prefabEntity);
        var relation = GetComponentIndex<T>();
        var relationship = GetRelationship(relation, target);

        if (!_archetypesByTypes.TryGetValue(isARelationship, out var archetypes))
            return;

        foreach (var archetype in archetypes)
        {
            var storage = archetype.GetStorage<T>(relationship);

            for (int i = 0; i < archetype.Count; i++)
            {
                var entity = archetype.Entities[i];

                ref var record = ref GetEntityRecord(entity);
                ref var component = ref storage[record.tableRow];

                if (modifyPrefabValueDelegate != null)
                {
                    modifyPrefabValueDelegate(ref component, value);
                    continue;
                }

                component = value;
            }
        }
    }

    public void SetPrefabValue<T1, T2>(Entity prefabEntity, T2 value, ModifyPrefabValue<T2>? modifyPrefabValueDelegate = null) where T1 : struct where T2 : struct
    {
        var isARelationship = _world.GetRelationship<IsA>(prefabEntity);
        var relation = GetComponentIndex<T1>();
        var target = GetComponentIndex<T2>();
        var relationship = GetRelationship(relation, target);

        if (!_archetypesByTypes.TryGetValue(isARelationship, out var archetypes))
            return;

        foreach (var archetype in archetypes)
        {
            var storage = archetype.GetStorage<T2>(relationship);

            for (int i = 0; i < archetype.Count; i++)
            {
                var entity = archetype.Entities[i];

                ref var record = ref GetEntityRecord(entity);
                ref var component = ref storage[record.tableRow];

                if (modifyPrefabValueDelegate != null)
                {
                    modifyPrefabValueDelegate(ref component, value);
                    continue;
                }

                component = value;
            }
        }
    }

    /// <returns>Wether new archetypeRefsList existed previously or not.</returns>
    public bool AddComponent(ulong typeIndex, Entity entity, out Archetype newArchetype, out int newTableRow, bool reuseTable = false, int customComponentsCapacity = -1)
    {
        int finalComponentsCapacity = customComponentsCapacity > 0 ? customComponentsCapacity : componentsCapacity;
        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var oldArchetypeRef = _archetypes[record.archetypeId];
        var oldArchetype = ((Archetype?)oldArchetypeRef.Target)!;

        if (HasComponent(typeIndex, ref record))
        {
#if DEBUG
            var entityName = GetEntityNameOrValue(entity);
            var componentName = GetComponentNameOrValue(typeIndex);
            throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
            newArchetype = oldArchetype;
            newTableRow = record.tableRow;
            return false;
#endif
        }

        var oldEdge = oldArchetype.GetTableEdge(typeIndex);
        newArchetype = ((Archetype?)oldEdge.Add?.Target)!;

        bool isNewArchetypeNull = newArchetype == null;
        if (isNewArchetypeNull)
        {
            SortedSet<ulong> newTypes = new(oldArchetype.components) { typeIndex };

            newTypes.Remove(entityType);
            newArchetype = TryGetArchetype(newTypes)!;

            Table newTable = null!;
            if (!reuseTable)
            {
                newTable = TryGetTable(newTypes)!;
            }
            newTable ??= reuseTable ? oldArchetype.Table : new Table(this, newTypes, finalComponentsCapacity, relationshipsCapacity);
            var newArchetypeRef = AddArchetype(newTable, newTypes, finalComponentsCapacity != singletonComponentCapacity ? -1 : singletonComponentCapacity);
            newArchetype ??= ((Archetype?)newArchetypeRef.Target)!;
            oldEdge.Add = newArchetypeRef;

            var newEdge = newArchetype.GetTableEdge(typeIndex);
            newEdge.Remove = oldArchetypeRef;
        }

        newTableRow = record.tableRow;
        int newArchetypeRow;
        if (!reuseTable)
        {
            newArchetypeRow = Table.MoveEntity(entity, record.archetypeRow, record.tableRow, oldArchetype, newArchetype!, out newTableRow);
            record.tableRow = newTableRow;
        }
        else
        {
            oldArchetype.Remove(record.archetypeRow, -1, false);
            newArchetypeRow = newArchetype!.Add(entity, out _, false);
        }
        record.archetypeId = newArchetype!.id;
        record.archetypeRow = newArchetypeRow;

        return isNewArchetypeNull;
    }

    public void AddComponent<T>(ulong typeIndex, Entity entity, T value, int customComponentsCapacity = -1) where T : struct
    {
#if DEBUG
        if (Unsafe.SizeOf<T>() == 1)
            throw new Exception($"Cannot add a component {typeof(T).Name} with no data .");
#endif

        Type type = null!;

        var fakeInstace = default(T);
        if (AutoResets<T>.Handler == null && fakeInstace is IAutoReset<T>)
        {
            type = typeof(T);
            var autoResetMethod = type.GetMethod(nameof(IAutoReset<T>.AutoReset))!;
            AutoResets<T>.Handler = (AutoReset<T>)Delegate.CreateDelegate(typeof(AutoReset<T>), null, autoResetMethod);
        }

        if (_locked)
        {
            type ??= typeof(T);
            ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
            var oldArchetypeRef = _archetypes[record.archetypeId];
            var oldArchetype = ((Archetype?)oldArchetypeRef!.Target)!;

            if (HasComponent(typeIndex, ref record))
            {
#if DEBUG
                var entityName = GetEntityNameOrValue(entity);
                var componentName = GetComponentNameOrValue(typeIndex);
                throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
                ref var item2 = ref oldArchetype.GetStorage<T>()[record.tableRow];
                item2 = value;
                return;
#endif
            }

            ref var item1 = ref AddArchetypeOperation<T>(entity, GetComponentIndex<T>(), ArchetypeOperationType.AddComponent);

            item1 = value;
            AutoResets<T>.Handler?.Invoke(ref item1, AutoResetState.OnAdd);

            return;
        }

        bool isNewArchetypeNull = AddComponent(typeIndex, entity, out var newArchetype, out int newTableRow, false, customComponentsCapacity);

        ref var item = ref newArchetype.GetStorage<T>()[newTableRow];
        item = fakeInstace;
        item = value;
        AutoResets<T>.Handler?.Invoke(ref item, AutoResetState.OnAdd);

        if (typeIndex != componentType)
            TryCallOnComponentAddSystems(entity, newArchetype, typeIndex);
    }

    public void AddTagEvent<T>(ulong typeIndex, Entity entity) where T : struct
    {
        if (TypeData.Components.Add(typeIndex))
            AddComponent<Component>(componentType, typeIndex, default);

        if (_locked)
        {
            ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];

            AddArchetypeOperation(entity, GetComponentIndex<T>(), ArchetypeOperationType.AddComponent);
            return;
        }

        if (AddComponent(typeIndex, entity, out var newArchetype, out var newTableRow, true))
        {
            if (new T() is IAutoReset<T>)
            {
                var type = typeof(T);

                var autoResetMethod = type.GetMethod(nameof(IAutoReset<T>.AutoReset))!;
                AutoResets<T>.Handler = (AutoReset<T>)Delegate.CreateDelegate(typeof(AutoReset<T>), null, autoResetMethod);
            }
        }
    }

    public EnumeratorSingleGetter<T> GetEvents<T>() where T : struct
    {
        var typeIndex = GetComponentIndex<T>();

        _archetypesByTypes.TryGetValue(typeIndex, out var archetypes);
        var archetype = archetypes?.First();

        return new EnumeratorSingleGetter<T>(this, archetype);
    }

    public Filter GetChildren(ulong entity)
    {
        if (!_childrenFilters.TryGetValue(entity, out var filter))
        {
            filter = _world.Filter().All<ChildOf>(entity).Build();
            _childrenFilters[entity] = filter;
        }

        return filter;
    }

    public void AddEvent<T>(ulong typeIndex, Entity entity, T value) where T : struct
    {
        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var oldArchetypeRef = _archetypes[record.archetypeId];
        var oldArchetype = ((Archetype?)oldArchetypeRef.Target)!;

        if (_locked)
        {
            var type = typeof(T);
            if (AutoResets<T>.Handler == null && default(T) is IAutoReset<T>)
            {
                var autoResetMethod = type.GetMethod(nameof(IAutoReset<T>.AutoReset))!;
                AutoResets<T>.Handler = (AutoReset<T>)Delegate.CreateDelegate(typeof(AutoReset<T>), null, autoResetMethod);
            }

            ref var item1 = ref AddArchetypeOperation<T>(entity, GetComponentIndex<T>(), ArchetypeOperationType.AddComponent);
            item1 = value;
            return;
        }

        if (HasComponent(typeIndex, ref record))
        {
#if DEBUG
            var entityName = GetEntityNameOrValue(entity);
            var componentName = GetComponentNameOrValue(typeIndex);
            throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
            return;
#endif
        }

        var oldEdge = oldArchetype.GetTableEdge(typeIndex);
        var newArchetype = ((Archetype?)oldEdge.Add?.Target)!;

        if (newArchetype == null)
        {
            SortedSet<ulong> newTypes = new(oldArchetype.components) { typeIndex };
            newTypes.Remove(entityType);
            var newArchetypeRef = AddArchetype(new Table(this, newTypes, componentsCapacity, relationshipsCapacity), newTypes);
            newArchetype = ((Archetype?)newArchetypeRef.Target)!;
            oldEdge.Add = newArchetypeRef;

            var newEdge = newArchetype.GetTableEdge(typeIndex);
            newEdge.Remove = oldArchetypeRef;

            if (AutoResets<T>.Handler == null && default(T) is IAutoReset<T>)
            {
                var type = typeof(T);
                var autoResetMethod = type.GetMethod(nameof(IAutoReset<T>.AutoReset))!;
                AutoResets<T>.Handler = (AutoReset<T>)Delegate.CreateDelegate(typeof(AutoReset<T>), null, autoResetMethod);
            }
        }

        var newArchetypeRow = Table.MoveEntity(entity, record.archetypeRow, record.tableRow, oldArchetype, newArchetype, out int newTableRow);
        record.archetypeRow = newArchetypeRow;
        record.tableRow = newTableRow;
        record.archetypeId = newArchetype.id;

        TryCallOnComponentAddSystems(entity, newArchetype, typeIndex);

        var storage = newArchetype.GetStorage<T>();
        storage[newTableRow] = default;

        ref var item = ref storage[newTableRow];
        item = value;
        AutoResets<T>.Handler?.Invoke(ref item, AutoResetState.OnAdd);
    }

    public bool RemoveComponent(ulong typeIndex, Entity entity, out Archetype newArchetype, out bool isNewArchetypeNull, bool reuseTable = false)
    {
        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var oldArchetypeRef = _archetypes[record.archetypeId];
        var oldArchetype = ((Archetype?)oldArchetypeRef.Target)!;

        if (!HasComponent(typeIndex, ref record))
        {
#if DEBUG
            var entityName = GetEntityNameOrValue(entity);
            var componentName = GetComponentNameOrValue(typeIndex);
            throw new Exception($"Cannot add non-existent component {componentName} to entity {entityName}.");
#else
            newArchetype = oldArchetype;
            isNewArchetypeNull = false;
            return false;
#endif
        }

        var oldEdge = oldArchetype.GetTableEdge(typeIndex);
        newArchetype = ((Archetype?)oldEdge.Remove?.Target)!;

        isNewArchetypeNull = newArchetype == null;
        if (isNewArchetypeNull && oldArchetype.components.Count > 1)
        {
            SortedSet<ulong> newTypes = new(oldArchetype.components);
            newTypes.Remove(typeIndex);
            newArchetype = TryGetArchetype(newTypes)!;

            Table newTable = null!;
            if (!reuseTable)
            {
                newTable = TryGetTable(newTypes)!;
            }
            newTable ??= reuseTable ? oldArchetype.Table : new Table(this, newTypes, componentsCapacity, relationshipsCapacity);
            var newArchetypeRef = AddArchetype(newTable, newTypes);
            newArchetype ??= ((Archetype?)newArchetypeRef.Target)!;
            oldEdge.Remove = newArchetypeRef;

            var newEdge = newArchetype.GetTableEdge(typeIndex);
            newEdge.Add = oldArchetypeRef;
        }

        int newArchetypeRow;
        if (!reuseTable)
        {
            newArchetypeRow = Table.MoveEntity(entity, record.archetypeRow, record.tableRow, oldArchetype, newArchetype!, out int tableRow);
            record.tableRow = tableRow;
        }
        else
        {
            oldArchetype.Remove(record.archetypeRow, -1, false);
            newArchetypeRow = newArchetype!.Add(entity, out _, false);
        }
        record.archetypeId = newArchetype!.id;
        record.archetypeRow = newArchetypeRow;

        TryCallOnComponentRemoveSystems(entity, oldArchetype, typeIndex);

        if (oldArchetype!.components.Count == 1)
            RemoveEntity(entity);

        return true;
    }

    public bool RemoveComponent<T>(ulong typeIndex, Entity entity) where T : struct
    {
        if (AutoResets<T>.Handler == null && default(T) is IAutoReset<T>)
        {
            var type = typeof(T);
            var autoResetMethod = type.GetMethod(nameof(IAutoReset<T>.AutoReset))!;
            AutoResets<T>.Handler = (AutoReset<T>)Delegate.CreateDelegate(typeof(AutoReset<T>), null, autoResetMethod);
        }

        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var oldArchetypeRef = _archetypes[record.archetypeId];
        var oldArchetype = ((Archetype?)oldArchetypeRef.Target)!;

        if (Unsafe.SizeOf<T>() > 1)
        {
            ref var item = ref oldArchetype.GetStorage<T>()[record.tableRow];
            AutoResets<T>.Handler?.Invoke(ref item, AutoResetState.OnRemove);
        }

        if (_locked)
        {
            if (!HasComponent(typeIndex, ref record))
            {
#if DEBUG
                var entityName = GetEntityNameOrValue(entity);
                var componentName = GetComponentNameOrValue(typeIndex);
                throw new Exception($"Cannot add non-existent component {componentName} to entity {entityName}.");
#else
                    return true;
#endif
            }

            AddArchetypeOperation(entity, typeIndex, ArchetypeOperationType.RemoveComponent);
            return true;
        }

        bool removed = RemoveComponent(typeIndex, entity, out var archetype, out bool isNewArchetypeNull);

        return removed;
    }

    public ref T GetComponent<T>(ulong typeIndex, Entity entity) where T : struct
    {
        GetComponent(typeIndex, entity, out var storage, out int row);
        return ref ((T[])storage)[row];
    }

    public void GetComponent(ulong typeIndex, Entity entity, out Array storage, out int tableRow)
    {
        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var archetypeRef = _archetypes[record.archetypeId];

#if DEBUG
        if (!HasComponent(typeIndex, ref record))
        {
            var entityName = GetEntityNameOrValue(entity);
            var componentName = GetComponentNameOrValue(typeIndex);
            throw new Exception($"Cannot get non-existent component {componentName} from entity{entityName}");
        }
#endif

        storage = ((Archetype?)archetypeRef!.Target)!.GetStorage(typeIndex);
        tableRow = record.tableRow;
    }

    public void RemoveEntity(Entity entity)
    {
        if (!IsEntityAlive(entity))
        {
#if DEBUG
            throw new Exception($"Cannot remove a dead entity {entity.value}");
#else
            return;
#endif
        }

        if (_locked)
        {
            AddArchetypeOperation(entity, 0, ArchetypeOperationType.RemoveEntity);
            return;
        }

        ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];
        var archetypeRef = _archetypes[record.archetypeId];
        var archetype = ((Archetype?)archetypeRef!.Target)!;

        HandleEntityDeletion(ref record);

        archetype.Remove(record.archetypeRow, record.tableRow);
        record.archetypeRow = record.tableRow = record.archetypeId = -1;
        record.entity = 0;

        _unusedIds.Enqueue(entity);
    }

    public void RemoveEvents<T>(ulong eventIndex) where T : struct
    {
        if (!_archetypesByTypes.TryGetValue(eventIndex, out var archetypes))
        {
#if DEBUG
            var componentName = GetComponentNameOrValue(eventIndex);
            throw new Exception($"Cannot remove non-existent event {eventIndex}");
#else
            return;
#endif
        }
        var archetype = archetypes.First();

        Table.RemoveEntities(archetype);

        for (int i = 0; i < archetype.Count; i++)
        {
            var entity = archetype.Entities[i];

            ref var record = ref _entityRecords[IdConverter.GetFirst(entity)];

            record.archetypeRow = record.tableRow = record.archetypeId = -1;
            record.entity = 0;

            _unusedIds.Enqueue(entity);
        }

        archetype.RemoveAll();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEntityAlive(Entity entity)
    {
        var id = IdConverter.GetFirst(entity);
        var record = _entityRecords[id];

        return record.archetypeRow >= 0 && IdConverter.GetSecond(record.entity) == IdConverter.GetSecond(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref EntityRecord GetEntityRecord(Entity entity)
    {
        return ref _entityRecords[IdConverter.GetFirst(entity)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref EntityRecord GetEntityRecord(uint index)
    {
        return ref _entityRecords[index];
    }

    public Filter GetFilter(Mask mask, Func<Archetypes, Mask, List<Archetype>, Filter> createFilter)
    {
        var hash = mask.GetHashCode();

        if (_filtersByHash.TryGetValue(hash, out var filterRefsList))
        {
            foreach (var filterRef in filterRefsList)
            {
                if (filterRef.Target is not Filter filter)
                    continue;

                var filterMask = filter.Mask;

                if (filterMask.hasTypes.SetEquals(mask.hasTypes) &&
                    filterMask.notTypes.SetEquals(mask.notTypes) &&
                    filterMask.anyTypes.SetEquals(mask.anyTypes))
                {
                    MaskPool.Add(mask);
                    return filter;
                }
            }
        }
        else
        {
            filterRefsList ??= new(1);
            _filtersByHash.Add(hash, filterRefsList);
        }

        var matchingArchetypes = new List<Archetype>();

#if DEBUG
        if (mask.hasTypes.Count == 0 && mask.anyTypes.Count == 0)
            throw new Exception("A filterRef can't have zero All and Any arguments");
#endif

        var firstType = mask.hasTypes.Count > 0 ? mask.hasTypes.First() : mask.anyTypes.First();
        if (_archetypesByTypes.ContainsKey(firstType))
            FindArchetypesCompatibleWith(mask, matchingArchetypes);

        var newFilter = createFilter(this, mask, matchingArchetypes);
        var newFilterRef = new WeakReference(newFilter);
        filterRefsList.Add(newFilterRef);

        TryAddFilterWithTargets(newFilter);

        return newFilter;
    }

    public Type GetDataRelationshipType(ulong relationship)
    {
        var firstValue = IdConverter.GetFirst(relationship);
        var secondValue = IdConverter.GetSecond(relationship);
        ulong finalComponentEntity;

        var firstComponentEntity = IdConverter.Compose(firstValue, uint.MaxValue, false);
        var finalValue = HasComponent(componentType, firstComponentEntity) && GetComponent<Component>(componentType, firstComponentEntity).size > 1
            ? firstValue : secondValue;
        finalComponentEntity = IdConverter.Compose(finalValue, uint.MaxValue, false);
        return TypeData.TypesByIndices[finalComponentEntity];
    }

    public bool IsDataRelationship(ulong relationship)
    {
        if (!IdConverter.IsRelationship(relationship))
            return false;

        var firstValue = IdConverter.GetFirst(relationship);
        var secondValue = IdConverter.GetSecond(relationship);

        var firstComponentEntity = IdConverter.Compose(firstValue, uint.MaxValue, false);
        var secondComponentEntity = IdConverter.Compose(secondValue, uint.MaxValue, false);

        bool firstHasData = firstValue != wildCard32 && HasComponent(componentType, firstComponentEntity) && GetComponent<Component>(componentType, firstComponentEntity).size > 1;
        bool secondHasData = secondValue != wildCard31 && HasComponent(componentType, secondComponentEntity) && GetComponent<Component>(componentType, secondComponentEntity).size > 1;
        return firstHasData || secondHasData;
    }

    public void AddEntityTag(Entity entity, Entity tag)
    {
        if (TypeData.Components.Add(tag))
            AddComponent<Component>(componentType, tag, default);

        ref var record = ref _entityRecords[IdConverter.GetFirst(tag)];
        record.SetIsTag(true);

        if (_locked)
        {
            if (HasComponent(tag, entity))
            {
#if DEBUG
                var entityName = GetEntityNameOrValue(entity);
                var componentName = GetComponentNameOrValue(tag);
                throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
                return;
#endif
            }

            AddArchetypeOperation(entity, tag, ArchetypeOperationType.AddComponent);
            return;
        }

        AddComponent(tag, entity, out _, out _, true);
    }

    public void TrRemoveArchetypesWithTag(Entity target, Entity tag)
    {
        RemoveComponent(tag, target, out _, out _, true);
    }

    public ulong GetComponentIndex(Type type)
    {
        if (!TypeData.IndicesByTypes.TryGetValue(type, out var index))
        {
            index = AddComponentEntity(type);
            TypeData.AddTypeAndIndex(type, index);
        }

        return index;
    }

    public ulong GetComponentIndex<T>() where T : struct
    {
        ulong index = TypeData<T>.index;
        if (index == 0)
        {
            var type = typeof(T);

            if (TypeData.IndicesByTypes.TryGetValue(type, out var exisitngIndex))
                return exisitngIndex;

            index = AddComponentEntity(type);
            TypeData.AddTypeAndIndex(type, index);
            TypeData<T>.index = index;

            if (Unsafe.SizeOf<T>() == 1)
                TypeData.Tags.Add(index);
        }

        return index;
    }

    public bool IsDataComponent(ulong component) =>
            !TypeData.Tags.Contains(component) || IsDataRelationship(component);

    /// <param name="redundantType">Specifies what relation and entity shouldn't be copied, meaning every eventIndex with specified relation or entity won't be copied)</param>
    public void CopyEntityComponents(Entity entityCopy, Entity entiyToCopy, bool copyChildren = false, ulong redundantType = 0)
    {
        var toCopyArchetype = GetArchetype(entiyToCopy);

        ref var instanceRecord = ref _entityRecords[IdConverter.GetFirst(entityCopy)];
        var instanceRow = instanceRecord.tableRow;

        var redundantFirst = IdConverter.GetFirst(redundantType);
        var redundantSecond = IdConverter.GetSecond(redundantType);

        Array entityStorage, instanceStorage;
        int entityRow;
        ulong prefabType = GetComponentIndex<Prefab>();

        foreach (var type in toCopyArchetype.components)
        {
            if (HasComponent(type, ref instanceRecord) || type == prefabType)
                continue;

            var currentFirst = IdConverter.GetFirst(type);
            var currentSecond = IdConverter.GetSecond(type);

            if (redundantFirst == currentFirst || redundantSecond == currentSecond)
                continue;

            ref var componentRecord = ref _entityRecords[currentFirst];

            bool reuseTable = !IsDataComponent(type);

            if (_locked)
            {
                if (reuseTable)
                {
                    AddArchetypeOperation(entityCopy, type, ArchetypeOperationType.AddComponent);
                    continue;
                }

                entityStorage = AddArchetypeOperation(entityCopy, type, ArchetypeOperationType.AddComponent, out entityRow);
                instanceStorage = toCopyArchetype.Table.GetStorage(type);
                Array.Copy(instanceStorage, instanceRow, entityStorage, entityRow, 1);

                continue;
            }
            AddComponent(type, entityCopy, out var newEntityArchetype, out entityRow, reuseTable);

            if (reuseTable)
                continue;

            entityStorage = newEntityArchetype.Table.GetStorage(type);
            instanceStorage = toCopyArchetype.Table.GetStorage(type);
            Array.Copy(instanceStorage, instanceRow, entityStorage, entityRow, 1);
        }

        if (copyChildren)
            TryAddInstanceOfChildren(entityCopy, entiyToCopy);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent(ulong typeIndex, ref EntityRecord record)
    {
        if (!_archetypesByTypes.TryGetValue(typeIndex, out var archetypes))
            return false;

        return record.archetypeRow != -1 && archetypes.Contains(((Archetype?)_archetypes[record.archetypeId].Target)!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent(ulong typeIndex, Entity entity)
    {
        var record = _entityRecords[IdConverter.GetFirst(entity)];

        if (!_archetypesByTypes.TryGetValue(typeIndex, out var archetypes))
            return false;

        return record.archetypeRow != -1 && archetypes.Contains(((Archetype?)_archetypes[record.archetypeId].Target)!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent(ulong typeIndex, Archetype archetype)
    {
        if (!_archetypesByTypes.TryGetValue(typeIndex, out var archetypes))
            return false;

        return archetypes.Contains(archetype);
    }

    private void TryAddFilterWithTargets(Filter filter)
    {
        var mask = filter.Mask;

        foreach (var type in mask.anyTypes)
        {
            if (!ComponentMightBeEntity(type))
                continue;

            if (!_filtersWithTagsByTypes.TryGetValue(type, out var filters))
            {
                filters = ListPool<Filter>.Get();
                _filtersWithTagsByTypes[type] = filters;
            }

            filters.Add(filter);
        }

        foreach (var type in mask.hasTypes)
        {
            if (!ComponentMightBeEntity(type))
                continue;

            if (!_filtersWithTagsByTypes.TryGetValue(type, out var filters))
            {
                filters = new();
                _filtersWithTagsByTypes[type] = filters;
            }

            filters.Add(filter);
        }
    }

    private bool ComponentMightBeEntity(ulong component)
    {
        return !HasComponent(componentType, component);
    }

    private void TryAddInstanceOfChildren(Entity entity, Entity instance)
    {
        var chilfOfRelationship = _world.GetRelationship<ChildOf>(instance);
        var redundantType = IdConverter.Compose(IdConverter.GetFirst(GetComponentIndex<ChildOf>()), 0, true);

        if (!_archetypesByTypes.TryGetValue(chilfOfRelationship, out var archetypes))
            return;

        foreach (var archetype in archetypes)
        {
            for (int i = 0; i < archetype.Count; i++)
            {
                var child = archetype.Entities[i];
                var newChild = AddEntity();

                var childName = !EntityHasName(child, instance)
                    ? string.Empty
                    : GetNameByEntity(child, instance);

                if (childName != string.Empty)
                    AddEntityName(newChild, entity, childName);

                _world.AddChildOf(newChild, entity);
                CopyEntityComponents(child, newChild, false, redundantType);
                TryAddInstanceOfChildren(newChild, child);
            }
        }
    }

    private void HandleEntityDeletion(ref EntityRecord record)
    {
        if (record.HasRelationships())
        {
            var childOfRelarionship = _world.FindRelationship<ChildOf, Wildcard>(record.entity);

            if (childOfRelarionship != 0)
            {
                var parent = childOfRelarionship.GetTarget();
                RemoveEntityName(record.entity, parent);
            }
        }
        if (EntityHasName(record.entity, 0))
        {
            RemoveEntityName(record.entity, 0);
        }
        if (record.IsTag())
        {
            RemoveComponentsFromEntities(record.entity, true);
        }
        if (record.IsRelation())
        {
            var component = IdConverter.Compose(IdConverter.GetFirst(record.entity), wildCard32, true);
            RemoveComponentsFromEntities(component, IsDataRelationship(component));
        }
        if (record.IsTarget())
        {
            var component = IdConverter.Compose(wildCard32, IdConverter.GetFirst(record.entity), true);
            RemoveComponentsFromEntities(component, IsDataRelationship(component));
        }

        TryRemoveFiltersWithTag(ref record);
        TryRemoveArchetypesWithTag(ref record);
    }

    private void TryRemoveFiltersWithTag(ref EntityRecord record)
    {
        if (!_filtersWithTagsByTypes.TryGetValue(record.entity, out var filtersWithTag))
            return;

        for (int i = 0; i < filtersWithTag.Count; i++)
        {
            var filter = filtersWithTag[i];
            var mask = filter.Mask;

            MaskPool.Add(mask);

            filtersWithTag[i] = null!;
        }

        ListPool<Filter>.Add(filtersWithTag);
        _filtersWithTagsByTypes.Remove(record.entity);
    }

    private void TryRemoveArchetypesWithTag(ref EntityRecord record)
    {
        if (!_archetypesByTypes.TryGetValue(record.entity, out var archetypes))
            return;

        HashSetPool<Archetype>.Add(archetypes);
        _archetypesByTypes.Remove(record.entity);
    }

    private void RemoveComponentsFromEntities(ulong component, bool reuseTable)
    {
        foreach (var archetype in _archetypesByTypes[component])
        {
            component = GetActualComponentId(archetype, component);

            //so that entites aren't being deleted while looping
            Lock();

            for (int i = 0; i < archetype.Count; i++)
            {
                var entity = archetype.Entities[i];
                AddArchetypeOperation(entity, component, ArchetypeOperationType.RemoveComponent);
            }

            Unlock();
        }
    }

    private ulong GetActualComponentId(Archetype archetype, ulong component)
    {
        var first = IdConverter.GetFirst(component);
        var second = IdConverter.GetSecond(component);

        if (first == wildCard32)
        {
            foreach (var type in archetype.components)
            {
                if (IdConverter.GetSecond(type) != second)
                    continue;

                return type;
            }
        }
        else if (second == wildCard31)
        {
            foreach (var type in archetype.components)
            {
                if (IdConverter.GetFirst(type) != first)
                    continue;

                return type;
            }
        }

        return component;

    }

    private Archetype? TryGetArchetype(SortedSet<ulong> types)
    {
        _archetypesByHashes.TryGetValue(types.GetCustomtHashCode(), out var archetypesList);

        if (archetypesList == null)
            return null;

        foreach (var archetypeRef in archetypesList)
        {
            var archetype = ((Archetype?)archetypeRef.Target)!;

            if (types.SetEquals(archetype.components))
                return archetype;
        }

        return null;
    }

    private Table? TryGetTable(SortedSet<ulong> types)
    {
        _tablesByHash.TryGetValue(types.GetCustomTableHashCode(this), out var tablesRefList);

        if (tablesRefList == null)
            return null;

        foreach (var tableRef in tablesRefList)
        {
            var table = (Table?)tableRef.Target!;
            if (types.MySetEquals(table.Components, this))
                return table;

        }
        return null;
    }

    private Entity AddComponentEntity(Type type = null!)
    {
        var entityValue = GetNewEntity(false);
        IdConverter.SetSecond(ref entityValue, uint.MaxValue);
        var entity = new Entity(entityValue, _world);
        var archetypeRef = _archetypes[0];
        var archetype = ((Archetype?)archetypeRef.Target)!;
        var archetypeRow = archetype.Add(entityValue, out int tableRow);
        var id = IdConverter.GetFirst(entityValue);

        if (_entityRecords.Length == _entityCount)
            Array.Resize(ref _entityRecords, (int)(_entityCount << 1));

        _entityRecords[id] = new EntityRecord() { archetypeRow = archetypeRow, tableRow = tableRow, archetypeId = archetype.id, entity = entityValue };

        AddComponent<Component>(componentType, entityValue, new() { type = type });
        return entity;
    }

    public void AddRelationship(Entity entity, Entity relation, Entity target)
    {
        ref var relationRecord = ref _entityRecords[IdConverter.GetFirst(relation)];
        ref var targetRecord = ref _entityRecords[IdConverter.GetFirst(target)];
        ref var entityRecord = ref _entityRecords[IdConverter.GetFirst(entity)];

        relationRecord.SetIsRelation(true);
        targetRecord.SetIsTarget(true);
        entityRecord.SetHasRelationsips(true);

        var relationship = GetRelationship(relation, target);

        TypeData.Tags.Add(relationship);

        if (_locked)
        {
            if (HasComponent(relationship, entity))
            {
#if DEBUG
                var entityName = GetEntityNameOrValue(entity);
                var componentName = GetComponentNameOrValue(relationship);
                throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
                return;
#endif
            }

            AddArchetypeOperation(entity, relationship, ArchetypeOperationType.AddComponent);
            return;
        }

        AddComponent(relationship, entity, out _, out _, true);
        TryCallOnComponentAddSystems(entity, (Archetype?)_archetypes[entityRecord.archetypeId].Target!, relationship);
    }

    public bool HasRelationship(Entity entity, Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);
        var relationship = IdConverter.Compose(relationId, targetId, true);

        if (relationship == wildCardRelationship)
        {
            var archetype = GetArchetype(entity);

            if (!_archetypesByTypes.TryGetValue(relationship, out var archetypes))
                return false;

            return archetypes.Contains(archetype);
        }
        else if (relationId == wildCard32)
        {
            var archetype = GetArchetype(entity);

            if (!_archetypesByTypes.TryGetValue(IdConverter.Compose(wildCard32, targetId, true), out var archetypes))
                return false;

            return archetypes.Contains(archetype);
        }
        else if (targetId == wildCard32)
        {
            var archetype = GetArchetype(entity);

            if (!_archetypesByTypes.TryGetValue(IdConverter.Compose(relationId, wildCard32, true), out var archetypes))
                return false;

            return archetypes != null && archetypes.Contains(archetype);
        }

        return HasComponent(relationship, entity);
    }

    public ulong GetRelationship(Entity relation, Entity target)
    {
        var relationId = IdConverter.GetFirst(relation);
        var targetId = IdConverter.GetFirst(target);
        return IdConverter.Compose(relationId, targetId, true);
    }

    public ref T AddDataRelationship<T>(Entity entity, Entity relation, Entity target) where T : struct
    {
        ref var relationRecord = ref _entityRecords[IdConverter.GetFirst(relation)];
        ref var targetRecord = ref _entityRecords[IdConverter.GetFirst(target)];
        ref var entityRecord = ref _entityRecords[IdConverter.GetFirst(entity)];

        relationRecord.SetIsRelation(true);
        targetRecord.SetIsTarget(true);
        entityRecord.SetHasRelationsips(true);

        var relationship = GetRelationship(relation, target);

        if (_locked)
        {
            if (HasComponent(relationship, entity))
            {
#if DEBUG
                var entityName = GetEntityNameOrValue(entity);
                var componentName = GetComponentNameOrValue(relationship);
                throw new Exception($"Entity {entityName} already has component {componentName}.");
#else
                var oldArchetype = (Archetype?)_archetypes[entityRecord.archetypeId].Target!;
                var oldStorage = oldArchetype.GetStorage<T>(relationship);
                return ref oldStorage[entityRecord.tableRow];
#endif
            }

            return ref AddArchetypeOperation<T>(entity, relationship, ArchetypeOperationType.AddComponent);
        }

        AddComponent(relationship, entity, out var archetype, out var newTableRow, false);
        TryCallOnComponentAddSystems(entity, (Archetype?)_archetypes[entityRecord.archetypeId].Target!, relationship);
        var storage = archetype.GetStorage<T>(relationship);
        return ref storage[newTableRow];
    }

    public void RemoveDataRelationship(Entity entity, Entity relation, Entity target)
    {
        var relationship = GetRelationship(relation, target);
        RemoveComponent(relationship, entity, out _, out _, false);
    }

    private void AddArchetypeOperation(Entity entity, ulong typeIndex, ArchetypeOperationType operationType, int index = -1)
    {
        _archetypeOperations.Add(new(operationType, typeIndex, entity, index));
    }

    private ref T AddArchetypeOperation<T>(Entity entity, ulong typeIndex, ArchetypeOperationType operationType) where T : struct
    {
        if (!_archetypeOperationStorages.TryGetValue(typeIndex, out var storage))
        {
            var type = typeof(T);
            storage = new(Array.CreateInstance(type, _talbeOperationComponentsCapacity), type);
        }

        _archetypeOperationStorages[typeIndex] = storage;
        ref T value = ref storage.Add<T>(out int index);
        AddArchetypeOperation(entity, typeIndex, operationType, index);

        return ref value;
    }

    private Array AddArchetypeOperation(Entity entity, ulong typeIndex, ArchetypeOperationType operationType, out int index)
    {
        if (!_archetypeOperationStorages.TryGetValue(typeIndex, out var storage))
        {
            var type = GetComponent<Component>(componentType, typeIndex).type;
            storage = new(Array.CreateInstance(type, _talbeOperationComponentsCapacity), type);
        }

        _archetypeOperationStorages[typeIndex] = storage;
        storage.Add(out index);
        AddArchetypeOperation(entity, typeIndex, operationType, index);

        return storage.array;
    }

    private void FindArchetypesCompatibleWith(Mask mask, List<Archetype> matchingArchetypes)
    {
        var matchingWithAnyArchetypes = new HashSet<Archetype>();

        if (mask.hasTypes.Count > 0)
        {
            var archetypesWithFirstHasType = _archetypesByTypes[mask.hasTypes.First()];
            foreach (var archetype in archetypesWithFirstHasType)
                if (MaskCompatibleWith(mask, archetype))
                    matchingArchetypes.Add(archetype);
        }

        foreach (var anyType in mask.anyTypes)
        {
            if (!_archetypesByTypes.TryGetValue(anyType, out var archetypesWithAnyType))
                continue;

            foreach (var archetypeWithAnyType in archetypesWithAnyType)
                matchingWithAnyArchetypes.Add(archetypeWithAnyType);
        }

        foreach (var matchingArchetype in matchingWithAnyArchetypes)
            matchingArchetypes.Add(matchingArchetype);
    }

    private bool ComponentsCompatibleOnlyWith(SortedSet<ulong> allComponents,
                                     SortedSet<ulong> noneComponents,
                                     SortedSet<ulong> anyComponents,
                                     Archetype archetype)
    {
        foreach (var noneComponent in noneComponents)
        {
            if (HasComponent(noneComponent, archetype))
                return false;
        }

        if (!ArchetypeHasAnyType(archetype, anyComponents))
            return false;

        foreach (var allComponent in allComponents)
        {
            if (IdConverter.IsRelationship(allComponent) && TypeCompatibleWith(archetype.components, allComponent))
                continue;

            if (!HasComponent(allComponent, archetype))
                return false;
        }

        return true;
    }

    private bool MaskCompatibleWith(Mask mask, Archetype archetype)
    {
        if (mask.hasTypes.Count > archetype.components.Count)
            return false;

        foreach (var notType in mask.notTypes)
        {
            if (HasComponent(notType, archetype))
                return false;
        }

        foreach (var hasType in mask.hasTypes)
        {
            if (IdConverter.IsRelationship(hasType) && TypeCompatibleWith(archetype.components, hasType))
                continue;

            if (!HasComponent(hasType, archetype))
                return false;
        }

        return true;
    }


    private bool MaskCompatibleWithAny(Mask mask, Archetype archetype)
    {
        if (mask.hasTypes.Count > archetype.components.Count)
            return false;

        foreach (var notType in mask.notTypes)
        {
            if (HasComponent(notType, archetype))
                return false;
        }

        if (mask.anyTypes.Count > 0 && !ArchetypeHasAnyType(archetype, mask.anyTypes))
            return false;

        foreach (var hasType in mask.hasTypes)
        {
            if (IdConverter.IsRelationship(hasType) && TypeCompatibleWith(archetype.components, hasType))
                continue;

            if (!HasComponent(hasType, archetype))
                return false;
        }

        return true;
    }

    private bool ArchetypeHasAnyType(Archetype archetype, SortedSet<ulong> anyTypes)
    {
        foreach (var anyType in anyTypes)
        {
            if (HasComponent(anyType, archetype))
                return true;
        }

        return false;
    }

    private bool TypeCompatibleWith(SortedSet<ulong> types, ulong relationship)
    {
        if (relationship == wildCard32)
            return true;

        var relation = IdConverter.GetFirst(relationship);
        var target = IdConverter.GetSecond(relationship);

        if (target == wildCard31)
        {
            foreach (var type in types)
            {
                if (!IdConverter.IsRelationship(type))
                    continue;

                if (relation == IdConverter.GetFirst(type))
                    return true;
            }

            return false;
        }
        else if (relation == wildCard32)
        {
            foreach (var type in types)
            {
                if (!IdConverter.IsRelationship(type))
                    continue;

                if (target == IdConverter.GetSecond(type))
                    return true;
            }

            return false;
        }
        else return false;
    }

    private WeakReference AddArchetype(Table table, SortedSet<ulong> types, int customCapacity = -1)
    {
        var finalCapacity = customCapacity == -1 ? GetArchetypeCapacity(types) : customCapacity;
        var archetype = new Archetype(table, _archetypes.Count, this, types, finalCapacity);
        var archetypeRef = new WeakReference(archetype);
        _archetypes.Add(archetypeRef);

        AddArchetypeByHash(archetypeRef, types.GetCustomtHashCode());
        AddTableByHash(table, table.Components.GetCustomTableHashCode(this));

        foreach (var type in types)
        {
            GetArchetypesWithType(type).Add(archetype);

            if (!IdConverter.IsRelationship(type) || type == componentType)
                continue;

            var relation = IdConverter.GetFirst(type);
            var target = IdConverter.GetSecond(type);

            var wildCardTarget = IdConverter.Compose(wildCard32, target, true);
            var relationWildCard = IdConverter.Compose(relation, wildCard32, true);

            GetArchetypesWithType(wildCardTarget).Add(archetype);
            GetArchetypesWithType(relationWildCard).Add(archetype);
            GetArchetypesWithType(wildCardRelationship).Add(archetype);
        }

        foreach (var onComponentSystem in _onComponentSystems)
        {
            if (!ComponentsCompatibleOnlyWith(onComponentSystem.allComponents,
                                         onComponentSystem.noneComponents,
                                         onComponentSystem.allComponents,
                                         archetype))
                continue;

            if (!_onComponentSystemsByArchetypeIds.TryGetValue(archetype.id, out var onComponentSystems))
            {
                onComponentSystems = new();
                _onComponentSystemsByArchetypeIds[archetype.id] = onComponentSystems;
            }

            onComponentSystems.Add(onComponentSystem);
        }

        foreach (var filterRefList in _filtersByHash.Values)
        {
            foreach (var filterRef in filterRefList)
            {
                if (filterRef.Target is not Filter filter)
                    continue;

                if (!MaskCompatibleWithAny(filter.Mask, archetype))
                    continue;

                filter.AddArchetype(archetype);
            }
        }

        return archetypeRef;
    }

    private int GetArchetypeCapacity(SortedSet<ulong> types)
    {
        foreach (var type in types)
        {
            if (IdConverter.IsRelationship(type))
                continue;

            return componentsCapacity;
        }

        return relationshipsCapacity;
    }

    private void AddArchetypeByHash(WeakReference archetypeRef, ulong hash)
    {
        if (_archetypesByHashes.TryGetValue(hash, out var archetypeRefsList))
        {
            archetypeRefsList.Add(archetypeRef);
            return;
        }

        archetypeRefsList = new(1) { archetypeRef };
        _archetypesByHashes.Add(hash, archetypeRefsList);
    }

    private void AddTableByHash(Table table, ulong hash)
    {
        if (_tablesByHash.TryGetValue(hash, out var tablesRefList))
        {
            tablesRefList.Add(new WeakReference(table));
            return;
        }

        tablesRefList = new() { new WeakReference(table) };
        _tablesByHash.Add(hash, tablesRefList);
    }

    private HashSet<Archetype> GetArchetypesWithType(ulong type)
    {
        if (!_archetypesByTypes.TryGetValue(type, out var archetypes))
        {
            archetypes = HashSetPool<Archetype>.Get();
            _archetypesByTypes.Add(type, archetypes);
        }

        return archetypes;
    }

    private ulong GetNewEntity(bool isRelation)
    {
        if (_unusedIds.Count == 0)
            return IdConverter.Compose(++_entityCount, 0, isRelation);

        var oldEntity = _unusedIds.Dequeue();
        IdConverter.SetSecond(ref oldEntity, (ushort)(IdConverter.GetSecond(oldEntity) + 1u));

        return oldEntity;
    }

    private void TryCallOnComponentAddSystems(
        Entity entity,
        Archetype archetype,
        ulong addedComponent)
    {
        if (!_onComponentSystemsByArchetypeIds.TryGetValue(archetype.id, out var onComponentSystems))
            return;

        var componentWildcard = IdConverter.Compose(
            IdConverter.GetFirst(addedComponent),
            wildCard32,
            true);
        var wildcardComponent = IdConverter.Compose(
            wildCard32,
            IdConverter.GetSecond(addedComponent),
            true);

        foreach (var onComponentSystem in onComponentSystems!)
        {
            var allComponents = onComponentSystem.allComponentsHashset;
            var anyComponents = onComponentSystem.anyComponents;
            bool allComponentsContains =
                allComponents.Contains(addedComponent) ||
                allComponents.Contains(componentWildcard) ||
                allComponents.Contains(wildcardComponent);

            if (allComponentsContains && anyComponents.Count == 0)
            {
                onComponentSystem.OnComponentAdd(entity);
                continue;
            }
            else if (anyComponents.Count == 0)
            {
                continue;
            }

            bool anyComponentsContains =
               anyComponents.Contains(addedComponent) ||
               anyComponents.Contains(componentWildcard) ||
               anyComponents.Contains(wildcardComponent);

            if (!allComponentsContains || !anyComponentsContains)
                continue;

            onComponentSystem.OnComponentAdd(entity);
        }
    }

    private void TryCallOnComponentRemoveSystems(
        Entity entity,
        Archetype archetype,
        ulong removedComponent)
    {
        if (!_onComponentSystemsByArchetypeIds.TryGetValue(archetype.id, out var onComponentSystems))
            return;

        var componentWildcard = IdConverter.Compose(
           IdConverter.GetFirst(removedComponent),
           wildCard32,
           true);
        var wildcardComponent = IdConverter.Compose(
            wildCard32,
            IdConverter.GetSecond(removedComponent),
            true);

        foreach (var onComponentSystem in onComponentSystems!)
        {
            var allComponents = onComponentSystem.allComponentsHashset;
            var anyComponents = onComponentSystem.anyComponents;
            bool allComponentsContains =
                allComponents.Contains(removedComponent) ||
                allComponents.Contains(componentWildcard) ||
                allComponents.Contains(wildcardComponent);

            if (allComponentsContains && anyComponents.Count == 0)
            {
                onComponentSystem.OnComponentRemove(entity);
                continue;
            }
            else if (anyComponents.Count == 0)
            {
                continue;
            }

            bool anyComponentsContains =
               anyComponents.Contains(removedComponent) ||
               anyComponents.Contains(componentWildcard) ||
               anyComponents.Contains(wildcardComponent);

            if (!allComponentsContains || !anyComponentsContains)
                continue;

            onComponentSystem.OnComponentRemove(entity);
        }
    }

    private void AddOnComponentSystems(IEnumerable<ISystem> systems)
    {
        foreach (var system in systems)
        {
            switch (system)
            {
                case OnComponentActionSystem onComponentActionSystem:
                    _onComponentSystems.Add(onComponentActionSystem);
                    break;
            }
        }
    }

#if DEBUG
    private string GetEntityNameOrValue(ulong entity)
    {
        var parent = IdConverter.GetSecond(_world.FindRelationship<ChildOf, Wildcard>(entity));

        if (!EntityHasName(entity, parent))
            return entity.ToString();

        return GetNameByEntity(entity, parent);
    }

    private string GetComponentNameOrValue(ulong component)
    {
        if (IdConverter.IsRelationship(component))
        {
            var first = _world.GetEntityFromIndex(IdConverter.GetFirst(component));
            var second = _world.GetEntityFromIndex(IdConverter.GetSecond(component));

            var firstName = GetComponentNameOrValue(first);
            var secondName = GetComponentNameOrValue(second);

            return string.Format("({0}, {1})", firstName, secondName);
        }

        var parent = IdConverter.GetSecond(_world.FindRelationship<ChildOf, Wildcard>(component));

        if (TypeData.TypesByIndices.TryGetValue(component, out var type))
            return type.Name;

        if (!EntityHasName(component, parent))
            return component.ToString();

        return GetNameByEntity(component, parent);
    }
#endif
}