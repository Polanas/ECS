using System.Runtime.CompilerServices;

namespace ECS;

internal interface IPool
{
    void TryCallAutoReset(Entity entity, AutoResetState state);
    object GetComponentAsObject(Array storage, int index);
    void SetComponent(Array storage, int index, object component);
}

internal class Pool<T> : IPool where T : struct
{
    public readonly AutoReset<T>? autoReset;
    private readonly ECSWorld _world;

    public Pool(ECSWorld world, AutoReset<T>? autoReset)
    {
        _world = world;
        this.autoReset = autoReset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryCallAutoReset(Entity enity, AutoResetState state)
    {
        if (autoReset == null)
            return;

        var component = _world.GetComponent<T>(enity);
        autoReset.Invoke(ref component.Value, state);
    }

    public object GetComponentAsObject(Array storage, int index)
    {
        return Unsafe.As<T[]>(storage)[index];
    }

    public void SetComponent(Array storage, int index, object component)
    {
        Unsafe.As<T[]>(storage)[index] = (T)component;
    }
}
