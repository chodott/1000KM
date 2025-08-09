using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> _objectPoolMap = new Dictionary<GameObject, Queue<GameObject>>();


    private void HandleReturn(GameObject prefab, GameObject gameObject)
    {
        IPoolingObject poolingObject = gameObject.GetComponent<IPoolingObject>();
        poolingObject.OnReturned -= HandleReturn;
        gameObject.SetActive(false);
        _objectPoolMap[prefab].Enqueue(gameObject);
    }

    public void CreateDefaultObjects(GameObject prefab, int spawnCount)
    {
        if (_objectPoolMap.ContainsKey(prefab) == false)
        {
            _objectPoolMap[prefab] = new Queue<GameObject>();
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.SetActive(false);
            _objectPoolMap[prefab].Enqueue( newObject );
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (_objectPoolMap[prefab].Count ==0 )
        {
            return Instantiate(prefab);
        }
        else
        {

            GameObject newObject = _objectPoolMap[prefab].Dequeue();
            IPoolingObject poolingObject = newObject.GetComponent<IPoolingObject>();
            poolingObject.OnReturned += HandleReturn;
            poolingObject.Activate(prefab);
            return newObject;
        }
    }
}
