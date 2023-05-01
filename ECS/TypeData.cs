using System.Runtime.CompilerServices;

namespace ECS;

public static class TypeData<T> where T : struct
{
    public static ulong index;
}

public static class TypeData
{
    public static Dictionary<ulong, Type> TypesByIndices { get; private set; } = new();
    public static Dictionary<Type, ulong> IndicesByTypes { get; private set; } = new();
    public static HashSet<ulong> Components { get; private set; } = new();
    public static HashSet<ulong> Tags { get; private set; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddTypeAndIndex(Type type, ulong index)
    {
        TypesByIndices[index] = type;
        IndicesByTypes[type] = index;
        Components.Add(index);
    }
}