namespace ECS
{
    public readonly struct Mask
    {
        public readonly SortedSet<ulong> hasTypes;
        public readonly SortedSet<ulong> anyTypes;
        public readonly SortedSet<ulong> notTypes;

        public Mask()
        {
            hasTypes = new();
            anyTypes = new();
            notTypes = new();
        }

        public void AddAll(ulong type)
        {
            hasTypes.Add(type);
        }

        public void AddAny(ulong type)
        {
            anyTypes.Add(type);
        }

        public void AddNot(ulong type)
        {
            notTypes.Add(type);
        }

        public void Clear()
        {
            hasTypes.Clear();
            anyTypes.Clear();
            notTypes.Clear();
        }

        public override int GetHashCode()
        {
            var hash = hasTypes.Count + anyTypes.Count + notTypes.Count;

            unchecked
            {
                foreach (var type in hasTypes)
                    hash = hash * 314159 + type.GetHashCode();
                foreach (var type in notTypes)
                    hash = hash * 314159 - type.GetHashCode();
                foreach (var type in anyTypes)
                    hash *= 314159 * type.GetHashCode();
            }

            return hash;
        }
    }
}