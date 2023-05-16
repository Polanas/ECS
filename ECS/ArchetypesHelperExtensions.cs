namespace ECS;

internal static class ArchetypesHelperExtensions
{
    internal static ulong GetCustomtHashCode(this SortedSet<ulong> hashSet)
    {
        ulong hash = (ulong)hashSet.Count;

        foreach (var value in hashSet)
        {
            hash = unchecked(hash * 314159ul + value);
        }

        return hash;
    }

    internal static ulong GetCustomTableHashCode(this SortedSet<ulong> types, Archetypes archetypes)
    {
        ulong hash = 0;

        foreach (var type in types)
        {
            if (!archetypes.IsDataComponent(type))
                continue;

            hash++;
        }

        foreach (var type in types)
        {
            if (!archetypes.IsDataComponent(type))
                continue;

            hash = unchecked(hash * 314159ul + type);
        }

        return hash;
    }

    internal static bool MySetEquals(this SortedSet<ulong> set, IEnumerable<ulong> otherSet, Archetypes archetypes)
    {
        SortedSet<ulong> asSorted = (otherSet as SortedSet<ulong>)!;

        var mine = set.GetEnumerator();
        var theirs = asSorted.GetEnumerator();
        bool mineEnded = !mine.MoveNext();
        bool theirsEnded = !theirs.MoveNext();
        bool mineHasNoData;
        bool theirsHasNoData;

        while (!mineEnded && !theirsEnded)
        {
            var mineCurrent = mine.Current;
            var theirsCurrent = theirs.Current;

            if (mineHasNoData = !archetypes.IsDataComponent(mineCurrent))
                mine.MoveNext();
            if (theirsHasNoData = !archetypes.IsDataComponent(theirsCurrent))
                theirs.MoveNext();

            if (mineHasNoData || theirsHasNoData)
                continue;

            if (mineCurrent != theirsCurrent)
                return false;

            mineEnded = !mine.MoveNext();
            theirsEnded = !theirs.MoveNext();
        }

        if (theirsEnded && mineEnded)
            return true;

        var notEndedEnum = theirsEnded ? mine : theirs;
        bool ended = false;

        while (!ended)
        {
            if (archetypes.IsDataComponent(notEndedEnum.Current))
                return false;

            ended = !notEndedEnum.MoveNext();
        }

        return true;
    }
}