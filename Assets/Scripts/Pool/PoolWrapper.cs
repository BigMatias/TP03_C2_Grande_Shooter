using UnityEngine;

public class PoolWrapper<T> : IPool where T : MonoBehaviour, IPooleable
{
    private readonly GenericPool<T> _inner;

    public PoolWrapper(T prefab, Transform parent, int initialSize, int maxSize)
    {
        _inner = new GenericPool<T>(prefab, parent, initialSize, maxSize);
    }

    public bool IsFull => _inner.IsFull;
    public int ActiveCount => _inner.ActiveCount;
    public void ReturnAll() => _inner.ReturnAll();
    public T Get() => _inner.Get();
    public void Return(T obj) => _inner.Return(obj);
}

