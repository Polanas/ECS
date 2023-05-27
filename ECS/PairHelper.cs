using System.Runtime.CompilerServices;

namespace ECS;

/// <summary>
/// T is supposed to be Pair1 or Pair2
/// </summary>
internal static class PairHelper
{
#if DEBUG
    private static readonly Dictionary<Type, bool> _cachedTypes = new();
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int GetSize<T>() where T : struct
    {
        var fakeInstance = new T();

        int* p = (int*)(&fakeInstance + 1);

        return *(p - 2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe ulong GetRelationship<T>() where T : struct
    {
        var fakeInstance = new T();

        ulong* p = (ulong*)((int*)(&fakeInstance + 1) - 2);

        var relation = IdConverter.GetFirst(*(p - 2));
        var target = IdConverter.GetFirst(*(p - 1));

        return IdConverter.Compose(relation, target, true);
    }

    internal static bool IsPair<T>() where T : struct =>
         default(T) is IPair;
}