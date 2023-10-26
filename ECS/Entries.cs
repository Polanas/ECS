using System.Runtime.CompilerServices;

namespace ECS;

public ref struct Entry
{
    public Entity entity;
}

public ref struct Entry<C>
    where C : struct
{
    public Entity entity;
    public ref C item;

    public ref C Item
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull() => Unsafe.IsNullRef(ref item);
}

public ref struct Entry<C1, C2>
    where C1 : struct
    where C2 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);
}

public ref struct Entry<C1, C2, C3>
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);
}

public ref struct Entry<C1, C2, C3, C4>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
{
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;
    public ref C4 item4;
    public Entity entity;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    public ref C4 Item4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item4;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull4() => Unsafe.IsNullRef(ref item4);
}

public ref struct Entry<C1, C2, C3, C4, C5>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;
    public ref C4 item4;
    public ref C5 item5;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    public ref C4 Item4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item4;
    }

    public ref C5 Item5
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item5;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull4() => Unsafe.IsNullRef(ref item4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull5() => Unsafe.IsNullRef(ref item5);
}

public ref struct Entry<C1, C2, C3, C4, C5, C6>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;
    public ref C4 item4;
    public ref C5 item5;
    public ref C6 item6;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    public ref C4 Item4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item4;
    }

    public ref C5 Item5
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item5;
    }

    public ref C6 Item6
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item6;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull4() => Unsafe.IsNullRef(ref item4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull5() => Unsafe.IsNullRef(ref item5);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull6() => Unsafe.IsNullRef(ref item6);
}

public ref struct Entry<C1, C2, C3, C4, C5, C6, C7>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;
    public ref C4 item4;
    public ref C5 item5;
    public ref C6 item6;
    public ref C7 item7;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    public ref C4 Item4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item4;
    }

    public ref C5 Item5
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item5;
    }

    public ref C6 Item6
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item6;
    }

    public ref C7 Item7
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item7;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull4() => Unsafe.IsNullRef(ref item4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull5() => Unsafe.IsNullRef(ref item5);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull6() => Unsafe.IsNullRef(ref item6);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull7() => Unsafe.IsNullRef(ref item7);
}

public ref struct Entry<C1, C2, C3, C4, C5, C6, C7, C8>
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    public Entity entity;
    public ref C1 item1;
    public ref C2 item2;
    public ref C3 item3;
    public ref C4 item4;
    public ref C5 item5;
    public ref C6 item6;
    public ref C7 item7;
    public ref C8 item8;

    public ref C1 Item1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item1;
    }

    public ref C2 Item2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item2;
    }

    public ref C3 Item3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item3;
    }

    public ref C4 Item4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item4;
    }

    public ref C5 Item5
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item5;
    }

    public ref C6 Item6
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item6;
    }

    public ref C7 Item7
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item7;
    }

    public ref C8 Item8
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref item8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull1() => Unsafe.IsNullRef(ref item1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull2() => Unsafe.IsNullRef(ref item2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull3() => Unsafe.IsNullRef(ref item3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull4() => Unsafe.IsNullRef(ref item4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull5() => Unsafe.IsNullRef(ref item5);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull6() => Unsafe.IsNullRef(ref item6);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull7() => Unsafe.IsNullRef(ref item7);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNull8() => Unsafe.IsNullRef(ref item8);
}