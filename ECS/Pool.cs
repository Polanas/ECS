using System.Runtime.CompilerServices;

namespace ECS;

internal interface IPool
{
    void CallAutoReset(Entity entity, AutoResetState state);
}

internal class Pool<T> : IPool where T : struct
{
    private readonly ECSWorld _world;
    private readonly AutoReset<T> _autoReset;

    public Pool(ECSWorld world, AutoReset<T> autoReset)
    {
        _world = world;
        _autoReset = autoReset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CallAutoReset(Entity enity, AutoResetState state)
    {
        var component = _world.GetComponent<T>(enity);
        _autoReset.Invoke(ref component.Value, state);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CallAutoReset(ref T c, AutoResetState state)
    {
        _autoReset.Invoke(ref c, state);
    }
}
