using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatGame : MonoBehaviour
{
    public GameObject background;
    public GameObject boatPrefab;
    public GameObject obstaclePrefab;
    public bool playerFlag;
    public int speedMode = 1;
    public float gameSpeed = 0.5f;

    void Start()
    {
        background = GameObject.Find("background");
        InvokeRepeating("InsertObstacles", 0.25f, 0.25f);

        GameEvents.current.onCollisionDetected += DestroyBoat;
    }

    void InsertObstacles()
    {
        if (Random.Range(-1f, 1f) > 0.25f)
        {
            Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1.1f, 0));
            newObstacleNormalizedPosition.z = 0;

            GameObject obstacle = ObjectPooling.SharedInstance.GetPooledObject("Log");
            if (obstacle != null)
            {
                obstacle.transform.position = newObstacleNormalizedPosition;
                obstacle.SetActive(true);
            }
        }
    }

    void DestroyBoat()
    {
        Debug.Log("Collision!");
        // gameObject.SetActive(false);
    }
    
}
