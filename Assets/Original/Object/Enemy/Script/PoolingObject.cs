
using System;
using UnityEngine;

public interface IPoolingObject
{
    public event Action<GameObject, GameObject> OnReturned;

    public GameObject OriginalPrefab {  get;}
    public void Activate(GameObject originalPrefab);
    public void Deactivate();
}