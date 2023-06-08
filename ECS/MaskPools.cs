using System.Runtime.CompilerServices;

namespace ECS;

internal static class MaskPool
{

    private static readonly Stack<Mask> _stack = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mask Get()
    {
        return _stack.Count > 0 ? _stack.Pop() : new Mask();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(Mask mask)
    {
        mask.Clear();
        _stack.Push(mask);
    }
}

internal static class ListMaskPool
{

    private static readonly Stack<ListMask> _stack = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ListMask Get()
    {
        return _stack.Count > 0 ? _stack.Pop() : new ListMask();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ListMask mask)
    {
        mask.Clear();
        _stack.Push(mask);
    }
}

