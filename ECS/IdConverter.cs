using System.Runtime.CompilerServices;

namespace ECS;

internal static class IdConverter
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetSecond(ulong entity)
    {
        return (uint)(entity >> 1) & 0x7FFFFFFFu;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetFirst(ulong entity)
    {
        return (uint)(entity >> 32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRelationship(ulong entity) 
    {
        return (entity & 1ul) == 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Compose(uint firstValue, uint secondValue, bool isRelation)
    {
        return (((ulong)firstValue) << 32) | (((ulong)secondValue << 1) & uint.MaxValue) | (isRelation ? 1ul : 0ul);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetFirst(ref ulong entity, uint id)
    {
        entity = (entity & 0xFFFFFFFFul) | ((ulong)id << 32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetSecond(ref ulong entity, uint generation)
    {
        entity = entity & (0x_FFFFFFFF00000001ul) | (((ulong)generation << 1 & uint.MaxValue));
    }
}