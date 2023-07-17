namespace ECS;

public enum OptionalState
{
    IsItem1,
    IsItem2,
    IsItem3,
}

internal readonly struct Optional<T1, T2>
{
    public T1? Item1 { get; init; }
    public T2? Item2 { get; init; }
    public OptionalState State { get; init; }

    public Optional(T1 item)
    {
        Item1 = item;
        State = OptionalState.IsItem1;
    }

    public Optional(T2 item)
    {
        Item2 = item;
        State = OptionalState.IsItem2;
    }
}

internal readonly struct Optional<T1, T2, T3>
{
    public T1? Item1 { get; init; }
    public T2? Item2 { get; init; }
    public T3? Item3 { get; init; }
    public OptionalState State { get; init; }

    public Optional(T1 item)
    {
        Item1 = item;
        State = OptionalState.IsItem1;
    }

    public Optional(T2 item)
    {
        Item2 = item;
        State = OptionalState.IsItem2;
    }

    public Optional(T3 item)
    {
        Item3 = item;
        State = OptionalState.IsItem2;
    }
}