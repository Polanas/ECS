using System.Runtime.CompilerServices;

namespace ECS;


internal static class ListPool<T>
{

    private static readonly Stack<List<T>> _stack = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> Get()
    {
        return _stack.Count > 0 ? _stack.Pop() : new List<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(List<T>? list)
    {
        if (list is null)
            return;

        list.Clear();
        _stack.Push(list);
    }
}
