namespace ECS;

public static class EmptyList<T>
{
    public static List<T> Instance { get; } = _isntance!;

    private static readonly List<T> _isntance = new();
}
