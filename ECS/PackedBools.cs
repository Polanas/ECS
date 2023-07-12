using System.Runtime.CompilerServices;

namespace ECS;

public struct PackedBoolByte
{
    private byte _data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(byte type, bool value) => _data = (byte)(value ? _data | (1 << type) : _data & 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Get(byte type) => (_data & (1 << type)) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _data = 0;
}

public struct PackedBoolInt
{
    public int _data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int type, bool value) => _data = value ? (_data | (1 << type)) : (_data & 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Get(int type) => (_data & (1 << type)) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _data = 0;
}