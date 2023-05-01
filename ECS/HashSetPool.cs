using System.Runtime.CompilerServices;

namespace ECS;


internal static class HashSetPool<T>
{

    private static readonly Stack<HashSet<T>> _stack = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<T> Get()
    {
        return _stack.Count > 0 ? _stack.Pop() : new HashSet<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(HashSet<T>? list)
    {
        if (list is null)
            return;

        list.Clear();
        _stack.Push(list);
    }
}
