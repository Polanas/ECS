using System.Runtime.CompilerServices;

namespace ECS;

public ref struct StackList<T> where T : unmanaged
{
    public Span<T> Items => _items;
    public int Count => _count;

    private int _count;
    private Span<T> _items;

    public StackList(Span<T> items)
    {
        _items = items;
    }

    public T this[int i]
    {
        get => _items[i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (_count < _items.Length)
        {
            _items[_count] = item;
            _count++;

            return;
        }

        AddWithResize(item);
    }

    public void Remove()
    {
        throw new NotImplementedException();
    }

    private void AddWithResize(T item)
    {
        int newSize = _count * 2;

        var newItems = new T[newSize];
        var newItemsSpan = new Span<T>(newItems);
        newItems.CopyTo(newItemsSpan);

        _items[_count] = item;

        _count++;
    }
}
