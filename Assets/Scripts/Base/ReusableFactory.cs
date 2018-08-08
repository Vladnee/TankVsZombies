using System.Collections.Generic;
using UnityEngine;

public class ReusableFactory
{
    List<ReusableGameObject> _enabled = new List<ReusableGameObject>();
    List<ReusableGameObject> _disabled = new List<ReusableGameObject>();
    private ReusableGameObject _prefab;
    private Transform _lair;

    private void _disableObject(ReusableGameObject obj)
    {
        _enabled.Remove(obj);
        _disabled.Add(obj);
    }

    public ReusableFactory(ReusableGameObject prefab, Transform lair)
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

    public ReusableGameObject Produce(Vector2 position)
    {
        ReusableGameObject gObj;
        if (_disabled.Count > 1)
        {
            gObj = _disabled.PopAt(0);
        }
        else
        {
            gObj = _instantiateNew();
        }
        _enabled.Add(gObj);
        gObj.Enable(position);
        return gObj;
    }
}
