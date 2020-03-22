using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float gameSpeed;

    void Start()
    {
        gameSpeed = GameObject.Find("BoatGameWrapper").GetComponent<BoatGame>().gameSpeed;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= gameSpeed * Time.deltaTime;
        transform.position = pos;
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}
