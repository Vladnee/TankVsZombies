using System;
using UnityEngine;

public class ReusableGameObject : MonoBehaviour
{
    public Action<ReusableGameObject> OnDisable = null;

    public void Disable()
    {
        gameObject.SetActive(false);
        if (OnDisable != null)
        {
            OnDisable(this);
        }
    }

    public virtual void Enable(Vector2 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
}
