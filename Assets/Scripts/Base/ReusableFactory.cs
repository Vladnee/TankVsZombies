using System.Collections.Generic;
using UnityEngine;

public class ReusableFactory<T> where T : ReusableGameObject
{
    List<ReusableGameObject> _enabled = new List<ReusableGameObject>();
    Queue<ReusableGameObject> _disabled = new Queue<ReusableGameObject>();
    private readonly T _prefab;
    private readonly Transform _lair;

    private void _disableObject(ReusableGameObject obj)
    {
        _enabled.Remove(obj);
        _disabled.Enqueue(obj);
    }

    public ReusableFactory(T prefab, Transform lair)
    {
        _prefab = prefab;
        _lair = lair;
    }

    private ReusableGameObject _instantiateNew()
    {
        ReusableGameObject gObj = GameObject.Instantiate(_prefab, _lair);
        gObj.OnDisable = _disableObject;
        return gObj;
    }

    public T Produce(Vector2 position)
    {
        ReusableGameObject gObj;
        if (_disabled.Count > 1)
        {
            gObj = _disabled.Dequeue();
        }
        else
        {
            gObj = _instantiateNew();
        }
        _enabled.Add(gObj);
        gObj.Enable(position);
        return gObj as T;
    }
}
