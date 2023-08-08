namespace ECS;

public readonly struct Mask
{
    public readonly SortedSet<ulong> allTypes;
    public readonly SortedSet<ulong> anyTypes;
    public readonly SortedSet<ulong> noneTypes;

    public Mask()
    {
        allTypes = new();
        anyTypes = new();
        noneTypes = new();
    }

    public void AddAll(ulong type)
    {
        allTypes.Add(type);
    }

    public void AddAny(ulong type)
    {
        anyTypes.Add(type);
    }

    public void AddNone(ulong type)
    {
        noneTypes.Add(type);
    }

    public void Clear()
    {
        allTypes.Clear();
        anyTypes.Clear();
        noneTypes.Clear();
    }

    public override int GetHashCode()
    {
        var hash = allTypes.Count + anyTypes.Count + noneTypes.Count;

        unchecked
        {
            foreach (var type in allTypes)
                hash = hash * 314159 + type.GetHashCode();
            foreach (var type in noneTypes)
                hash = hash * 314159 - type.GetHashCode();
            foreach (var type in anyTypes)
                hash *= 314159 * type.GetHashCode();
        }

        return hash;
    }
}

/// <summary>
/// It's used alongsider regular masks, as having lists of types is sometimes more convenient
/// </summary>
public readonly struct ListMask
{
    public readonly List<ulong> allTypes;
    public readonly List<ulong> anyTypes;
    public readonly List<ulong> noneTypes;

    public ListMask()
    {
        allTypes = new();
        anyTypes = new();
        noneTypes = new();
    }

    public void AddAll(ulong type)
    {
        allTypes.Add(type);
    }

    public void AddAny(ulong type)
    {
        anyTypes.Add(type);
    }

    public void AddNone(ulong type)
    {
        noneTypes.Add(type);
    }

    public void Clear()
    {
        allTypes.Clear();
        anyTypes.Clear();
        noneTypes.Clear();
    }
}