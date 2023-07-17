namespace ECS;

public interface IGroupSystem
{
    void OnActivate();
    void OnDeactivate();
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
    private readonly int _updateSystemsCount;
    private readonly int _postUpdateSystemsCount;
    private readonly int _groupSystemsCount;
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