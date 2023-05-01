using System.Runtime.InteropServices;

namespace ECS;

internal static class InjectHelper
{
    internal static unsafe ulong GetComponentType(IInjectedFilterParameter parameter)
    {
        GCHandle handle = GCHandle.Alloc(parameter, GCHandleType.Pinned);
        var component = *(ulong*)handle.AddrOfPinnedObject();

        handle.Free();
        return component;
    }
}