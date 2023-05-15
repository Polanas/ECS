using System.Reflection;
using System.Runtime.InteropServices;

namespace ECS;

public interface ISystem { }

public interface IEvent { }

public interface ISingletonEvent { }

public interface IUpdateSystem : ISystem
{
    void Update();
}

public interface IPostUpdateSystem : ISystem
{
    void PostUpdate();
}

public interface IInitSystem : ISystem
{
    void Init(ECSSystems systems);
}

public interface IPreInitSystem : ISystem
{
    void PreInit(ECSSystems systems);
}

public interface IDestroySystem : ISystem
{
    void Destroy(ECSSystems systems);
}

public interface IDataInject
{
    internal void Fill(ECSSystems systems);
}

public interface ICustomDataInject
{
    internal void Fill(object[] injects);
}

public interface IAll { }

public interface INone { }

public interface IAny { }

public interface IInjectedFilterParameter
{
    ulong GetComponent();
}

[StructLayout(LayoutKind.Sequential)]
public struct All<T> : IInjectedFilterParameter, IAll where T : struct
{
    public ulong index;

    public All()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

[StructLayout(LayoutKind.Sequential)]
public struct All<T1, T2> : IInjectedFilterParameter, IAll where T1 : struct where T2 : struct
{
    public ulong index;

    public All()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct None<T> : IInjectedFilterParameter, INone where T : struct
{
    public ulong index;

    public None()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

public struct None<T1, T2> : IInjectedFilterParameter, INone where T1 : struct where T2 : struct
{
    public ulong index;

    public None()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct Any<T> : IInjectedFilterParameter, IAny where T : struct
{
    public ulong index;

    public Any()
    {
        var world = ECSWorld.Instance!;
        index = world.IndexOf<T>();
    }

    public ulong GetComponent() => index;
}

public struct Any<T1, T2> : IInjectedFilterParameter, IAny where T1 : struct where T2 : struct
{
    public ulong index;

    public Any()
    {
        var world = ECSWorld.Instance!;
        index = world.GetRelationship<T1, T2>();
    }

    public ulong GetComponent() => index;
}

public struct FilterInject : IDataInject
{
    public Filter value = null!;

    private FilterBuilder _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C> : IDataInject
    where C : struct
{
    public Filter<C> value = null!;

    private FilterBuilder<C> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C> GetEnumerator()
    {
        return new Enumerator<C>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2> : IDataInject
    where C1 : struct
    where C2 : struct
{
    public Filter<C1, C2> value = null!;

    private FilterBuilder<C1, C2> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2> GetEnumerator()
    {
        return new Enumerator<C1, C2>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    public Filter<C1, C2, C3> value = null!;

    private FilterBuilder<C1, C2, C3> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    public Filter<C1, C2, C3, C4> value = null!;

    private FilterBuilder<C1, C2, C3, C4> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    public Filter<C1, C2, C3, C4, C5> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    public Filter<C1, C2, C3, C4, C5, C6> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6, C7> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    public Filter<C1, C2, C3, C4, C5, C6, C7> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    public Enumerator<C1, C2, C3, C4, C5, C6, C7> GetEnumerator()
    {
        return new Enumerator<C1, C2, C3, C4, C5, C6, C7>(value.Archetypes, value.archetypesList);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct FilterInject<C1, C2, C3, C4, C5, C6, C7, C8> : IDataInject
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    public Filter<C1, C2, C3, C4, C5, C6, C7, C8> value = null!;

    private FilterBuilder<C1, C2, C3, C4, C5, C6, C7, C8> _builder;
    private readonly List<ulong>? _hasTypes;
    private readonly List<ulong>? _notTypes;
    private readonly List<ulong>? _anyTypes;

    public FilterInject(params IInjectedFilterParameter[] parameters)
    {
        _hasTypes = ListPool<ulong>.Get();
        _anyTypes = ListPool<ulong>.Get();
        _notTypes = ListPool<ulong>.Get();

        foreach (var parameter in parameters)
        {
            var component = parameter.GetComponent();

            if (parameter is IAll)
                _hasTypes.Add(component);
            else if (parameter is INone)
                _notTypes.Add(component);
            else if (parameter is IAny)
                _anyTypes.Add(component);
        }
    }

    public void Fill(ECSSystems systems)
    {
        var arhcetypes = systems.World.Archetypes;
        _builder = new(arhcetypes);

        AddTypes(_hasTypes, _builder.mask.hasTypes);
        AddTypes(_notTypes, _builder.mask.notTypes);
        AddTypes(_anyTypes, _builder.mask.anyTypes);

        value = _builder.Build();

        ListPool<ulong>.Add(_hasTypes);
        ListPool<ulong>.Add(_anyTypes);
        ListPool<ulong>.Add(_notTypes);
    }

    private void AddTypes(List<ulong>? types, SortedSet<ulong> maskTypes)
    {
        if (types is null)
            return;

        foreach (var type in types)
            maskTypes.Add(type);
    }
}

public struct SharedInject<T> : IDataInject where T : class
{
    public T Instance { get; private set; }

    public void Fill(ECSSystems systems)
    {
        Instance = systems.GetShared<T>();
    }
}

public struct CustomInject<T> : ICustomDataInject where T : class
{
    public T Instance { get; private set; }

    public void Fill(object[] injects)
    {
        if (injects.Length == 0)
            return;

        var type = typeof(T);

        foreach (var inject in injects)
        {
            if (!type.IsInstanceOfType(inject))
                continue;

            Instance = (T)inject;
            break;
        }
    }
}

public interface IOnComponentActionSystem
{
    void OnComponentAdd(Entity entity);
    void OnComponentRemove(Entity entity);
}

public abstract class OnComponentActionSystem : IOnComponentActionSystem, ISystem
{
    internal SortedSet<ulong> allComponents = null!;
    internal HashSet<ulong> allComponentsHashset = null!;
    internal SortedSet<ulong> noneComponents = null!;
    internal SortedSet<ulong> anyComponents = null!;
    internal HashSet<ulong> anyComponentsHashset = null!;
    private bool _implementsOnComponentRemove;

    public OnComponentActionSystem()
    {
        allComponents = new();
        noneComponents = new();
        anyComponents = new();

        anyComponentsHashset = new();
        allComponentsHashset = new();

        var type = GetType();
        var onComponentRemove = type.GetMethod(nameof(OnComponentRemove));
        _implementsOnComponentRemove = 
            onComponentRemove!.DeclaringType == type ||
            onComponentRemove.GetBaseDefinition().DeclaringType == type;
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

    protected void Any<T>() where T : struct
    {
        var component = ECSWorld.Instance!.IndexOf<T>();
        AddAny(component);
    }

    protected void Any<T1, T2>() where T1 : struct where T2 : struct
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
    }

    private void AddAny(ulong component)
    {
        anyComponents.Add(component);
        anyComponentsHashset.Add(component);
    }
}

internal delegate void OnComponentActionHandler(Entity entity);

public interface IGroupSystem
{
    void OnActivate();
    void OnDeactivate();
}

public class ECSSystems
{
    public ECSWorld World => _world;
    internal List<ISystem> Systems => _allSystems;

    private readonly ECSWorld _world;
    private readonly List<ISystem> _allSystems;
    private readonly object _sharedData;
    private IUpdateSystem[] _updateSystems = null!;
    private IPostUpdateSystem[] _postUpdateSystems = null!;
    private IDestroySystem[] _destroySystems = null!;
    private readonly Dictionary<Type, ISystem> _systemsByTypes;
    private readonly Dictionary<string, GroupSystem> _groupSystems;
    private int _updateSystemsCount;
    private int _postUpdateSystemsCount;
    private int _destroySystemsCount;
    private bool _inited;

    public ECSSystems(ECSWorld world, object sharedData = null!)
    {
        _allSystems = new();
        _systemsByTypes = new();
        _groupSystems = new();
        _sharedData = sharedData;
        _world = world;
    }

    public T GetShared<T>() => (T)_sharedData;

    public ECSSystems DeleteHere<T>() where T : struct
    {
        _allSystems.Add(new DeleteHereSystem<T>());
        _updateSystemsCount++;

        return this;
    }

    public ECSSystems Add<T>() where T : class, ISystem, new()
    {
        var system = new T();
        _systemsByTypes.Add(typeof(T), system);

        _allSystems.Add(system);
        if (system is IUpdateSystem)
            _updateSystemsCount++;
        if (system is IPostUpdateSystem)
            _postUpdateSystemsCount++;
        if (system is IDestroySystem)
            _destroySystemsCount++;

        return this;
    }

    public ECSSystems Add<T>(T system) where T : class, ISystem
    {
        _systemsByTypes.Add(typeof(T), system);
        _allSystems.Add(system);

        if (system is IUpdateSystem)
            _updateSystemsCount++;
        if (system is IPostUpdateSystem)
            _postUpdateSystemsCount++;
        if (system is IDestroySystem)
            _destroySystemsCount++;

        return this;
    }

    public ECSSystems AddGroup(string name, bool defaultState, params ISystem[] systems)
    {
        Add(new GroupSystem(name, defaultState, systems));

        return this;
    }

    public bool SetGroupState(string name, bool state)
    {
        if (!_groupSystems.TryGetValue(name, out var groupSystem))
#if !DEBUG
            throw new Exception($"A group system with the name {name} doesn't exist");
#else
            return false;
#endif

        groupSystem.SetState(state);
        return true;
    }

    public void Update()
    {
#if DEBUG
        if (!_inited)
            throw new Exception("Cannot update systems, as Init method have not been called.");
#endif

        for (int i = 0; i < _updateSystemsCount; i++)
        {
            _updateSystems[i].Update();

#if DEBUG
            if (_world.Archetypes.EntityArchetype.Count > 0)
                throw new Exception($"Empty entities were created during update of {_updateSystems[i].GetType().Name}");
#endif
        }

        for (int i = 0; i < _postUpdateSystemsCount; i++)
        {
            _postUpdateSystems[i].PostUpdate();

#if DEBUG
            if (_world.Archetypes.EntityArchetype.Count > 0)
                throw new Exception($"Empty entities were created during post update of {_postUpdateSystems[i].GetType().Name}");
#endif
        }
    }

    public T GetSystem<T>() where T : class, ISystem =>
     (_systemsByTypes[typeof(T)] as T)!;

    public void Init()
    {
        _inited = true;
        _world.AddSystems(this);

        if (_updateSystemsCount > 0)
            _updateSystems = new IUpdateSystem[_updateSystemsCount];
        if (_postUpdateSystemsCount > 0)
            _postUpdateSystems = new IPostUpdateSystem[_postUpdateSystemsCount];
        if (_destroySystemsCount > 0)
            _destroySystems = new IDestroySystem[_destroySystemsCount];

        foreach (var system in _allSystems)
        {
            if (system is not IPreInitSystem preInitSystem)
                continue;

            preInitSystem.PreInit(this);

#if DEBUG
            if (_world.Archetypes.EntityArchetype.Count > 0)
                throw new Exception($"Empty entities were created during preinit of {system.GetType().Name}");
#endif
        }

        int updateSystemInex = 0;
        int postUpdateSystemIndex = 0;
        foreach (var system in _allSystems)
        {
            if (system is IInitSystem InitSystem)
            {
                InitSystem.Init(this);

#if DEBUG
                if (_world.Archetypes.EntityArchetype.Count > 0)
                    throw new Exception($"Empty entities were created during init of {system.GetType().Name}");
#endif
            }

            if (system is IUpdateSystem updateSystem)
                _updateSystems[updateSystemInex++] = updateSystem;
            if (system is IPostUpdateSystem postUpdateSystem)
                _postUpdateSystems[postUpdateSystemIndex++] = postUpdateSystem;
        }

        foreach (var system in _allSystems)
        {
            if (system is GroupSystem groupSystem)
                _groupSystems[groupSystem.Name] = groupSystem;
        }
    }

    public ECSSystems Inject(params object[] injects)
    {
        injects ??= Array.Empty<object>();

        for (int i = 0; i < _allSystems.Count; i++)
        {
            var system = _allSystems[i];

            foreach (var field in system.GetType().GetFields(BindingFlags.Public |
                                                              BindingFlags.NonPublic |
                                                              BindingFlags.Instance))
            {
                if (field.IsStatic)
                    continue;
                if (TryInjectInSystemGropus(field, system, injects))
                    continue;
                if (TryInjectBuildIns(field, system))
                    continue;
                if (TryInjectCustoms(field, system, injects))
                    continue;
            }
        }

        return this;
    }

    public void Destroy()
    {
        for (int i = 0; i < _destroySystemsCount; i++)
        {
            _destroySystems[i].Destroy(this);
        }
    }

    private bool TryInjectBuildIns(FieldInfo fieldInfo, ISystem system)
    {
        if (!typeof(IDataInject).IsAssignableFrom(fieldInfo.FieldType))
            return false;

        var instance = (IDataInject)fieldInfo.GetValue(system)!;
        instance.Fill(this);
        fieldInfo.SetValue(system, instance);

        return true;
    }

    private bool TryInjectCustoms(FieldInfo fieldInfo, ISystem system, object[] injects)
    {
        if (!typeof(ICustomDataInject).IsAssignableFrom(fieldInfo.FieldType))
            return false;

        var instance = (ICustomDataInject)fieldInfo.GetValue(system)!;
        instance.Fill(injects);
        fieldInfo.SetValue(system, instance);

        return true;
    }

    private bool TryInjectInSystemGropus(FieldInfo fieldInfo, ISystem system, object[] injects)
    {
        if (system is not GroupSystem groupSystem || fieldInfo.FieldType != typeof(ISystem[]))
            return false;

        foreach (var nestedSystem in groupSystem.Systems)
        {
            foreach (var field in nestedSystem.GetType().GetFields(BindingFlags.Public |
                                                             BindingFlags.NonPublic |
                                                             BindingFlags.Instance))
            {
                if (field.IsStatic)
                    continue;
                if (TryInjectBuildIns(field, nestedSystem))
                    continue;
                if (TryInjectCustoms(field, nestedSystem, injects))
                    continue;
            }
        }

        return true;
    }
}

public sealed class DeleteHereSystem<T> : IPreInitSystem, IUpdateSystem where T : struct
{
    private Filter _filter = null!;
    private ECSWorld _world = null!;
    private bool _isEvent;

    public void Update()
    {
        if (_isEvent)
        {
            _world.Archetypes.RemoveEvents<T>(_world.IndexOf<T>());
            return;
        }

        foreach (var entry in _filter)
        {
            entry.entity.Remove<T>();
        }
    }

    public void PreInit(ECSSystems systems)
    {
        _world = systems.World;
        _isEvent = default(T) is IEvent;

        if (!_isEvent)
            _filter = _world.Filter().All<T>().Build();
    }
}

public sealed class GroupSystem :
    IPreInitSystem,
    IInitSystem,
    IUpdateSystem,
    IDestroySystem
{
    public ISystem[] Systems => _allSystems;
    public string Name => _name;

    private readonly string _name;
    private readonly ISystem[] _allSystems;
    private readonly IUpdateSystem[] _updateSystems = null!;
    private readonly IPostUpdateSystem[] _postUpdateSystems = null!;
    private readonly IGroupSystem[] _groupSystems = null!;
    private int _updateSystemsCount;
    private int _postUpdateSystemsCount;
    private int _groupSystemsCount;
    private bool _state;

    public GroupSystem(string name, bool defaultState, params ISystem[] systems)
    {
        _name = name;
        _state = defaultState;
        _allSystems = systems;

        foreach (var system in _allSystems)
        {
            if (system is IUpdateSystem)
                _updateSystemsCount++;
            if (system is IGroupSystem)
                _groupSystemsCount++;
            if (system is IPostUpdateSystem)
                _postUpdateSystemsCount++;
        }

        if (_updateSystemsCount > 0)
            _updateSystems = new IUpdateSystem[_updateSystemsCount];
        if (_groupSystemsCount > 0)
            _groupSystems = new IGroupSystem[_groupSystemsCount];
        if (_postUpdateSystemsCount > 0)
            _postUpdateSystems = new IPostUpdateSystem[_postUpdateSystemsCount];

        _updateSystemsCount = _groupSystemsCount = _postUpdateSystemsCount = 0;

        foreach (var system in _allSystems)
        {
            if (system is IUpdateSystem updateSystem)
                _updateSystems[_updateSystemsCount++] = updateSystem;
            if (system is IGroupSystem groupSystem)
                _groupSystems[_groupSystemsCount++] = groupSystem;
            if (system is IPostUpdateSystem postUpdateSystem)
                _postUpdateSystems[_postUpdateSystemsCount++] = postUpdateSystem;
        }
    }

    public void PreInit(ECSSystems systems)
    {
        foreach (var system in _allSystems)
        {
            if (system is IPreInitSystem preInitSystem)
                preInitSystem.PreInit(systems);
        }
    }

    public void Init(ECSSystems systems)
    {
        foreach (var system in _allSystems)
        {
            if (system is IInitSystem initSystem)
                initSystem.Init(systems);
        }
    }

    public void Update()
    {
        if (!_state)
            return;

        for (int i = 0; i < _updateSystemsCount; i++)
        {
            _updateSystems[i].Update();
        }

        for (int i = 0; i < _postUpdateSystemsCount; i++)
        {
            _postUpdateSystems[i].PostUpdate();
        }
    }

    public void Destroy(ECSSystems systems)
    {
        foreach (var system in _allSystems)
        {
            if (system is IDestroySystem destroySystem)
                destroySystem.Destroy(systems);
        }
    }

    internal void SetState(bool state)
    {
        if (state == _state)
            return;
        _state = state;


        if (_state)
        {
            for (int i = 0; i < _groupSystemsCount; i++)
            {
                var groupSystem = _groupSystems[i];
                groupSystem.OnActivate();
            }

            return;
        }

        for (int i = 0; i < _groupSystemsCount; i++)
        {
            var groupSystem = _groupSystems[i];
            groupSystem.OnDeactivate();
        }
    }
}