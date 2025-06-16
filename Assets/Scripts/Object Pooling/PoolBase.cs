using System;
using System.Collections.Generic;

public class PoolBase<T>
{
    private readonly Func<T> _preloadFunc;
    private readonly Action<T> _getAction;
    private readonly Action<T> _returnAction;
    private Queue<T> _pool = new Queue<T>();
    private List<T> _active = new List<T>();
    private bool _canExpand = false;

    public PoolBase(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount, bool canExpand)
    {
        _preloadFunc = preloadFunc;
        _getAction = getAction;
        _returnAction = returnAction;

        if (preloadFunc == null)
        {
            return;
        }

        for (int i = 0; i < preloadCount; i++)
        {
            Return(preloadFunc());
        }

        _canExpand = canExpand;
    }

    public T Get()
    {
        T item = default;
        if (_canExpand == true)
        {
            item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
        }
        else
        {
            if (_pool.Count <= 0)
                Return(_active[0]);

            item = _pool.Dequeue();
        }

        _getAction(item);
        _active.Add(item);

        return item;
    }

    public void Return(T item)
    {
        _returnAction(item);
        _pool.Enqueue(item);
        _active.Remove(item);
    }

    public void ReturnAll()
    {
        foreach (T item in _active.ToArray())
        {
            Return(item);
        }
    }
}
