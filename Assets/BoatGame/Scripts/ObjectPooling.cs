using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: this is all super generic based on following website
// https://www.raywenderlich.com/847-object-pooling-in-unity
public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;
    // public List<GameObject> pooledObjects;
    Dictionary<string, List<GameObject>> pooledObjects;
    public List<GameObject> prefabsToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < prefabsToPool.Count; i++)
        {
            InstantiateOjbectsOfType(prefabsToPool[i]);
        }
    }

    void Update()
    {
        
    }

    void InstantiateOjbectsOfType(GameObject objectType)
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        { 
            GameObject newObject = Instantiate(objectType);
            newObject.SetActive(false);
            objects.Add(newObject);
        }
        pooledObjects.Add(objectType.name, objects);
    }

    public GameObject GetPooledObject(string objectName)
    {
        for (int i = 0; i < pooledObjects[objectName].Count; i++)
        {
            if (!pooledObjects[objectName][i].activeInHierarchy)
            {
                return pooledObjects[objectName][i];
            }
        }   
        return null;
    }
}
