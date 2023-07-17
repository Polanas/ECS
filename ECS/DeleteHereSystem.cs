namespace ECS;

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