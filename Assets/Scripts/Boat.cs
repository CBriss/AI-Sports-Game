﻿using UnityEngine;

public class Boat : GameComponent
{
    private float gameSpeed;
    private Vector2 bounds;
    public int score;
    public float distanceTraveled;

    public NeuralNet brain;

    void Start()
    {
        gameSpeed = GameObject.Find("BoatGameWrapper").GetComponent<BoatGame>().gameSpeed;
        distanceTraveled = 0;
    }

    void Update()
    {
        MoveFromThinking();
        UpdateScore();
    }

    void MoveFromInput()
    {
        Vector3 boatPos = transform.position;
        if (Input.GetKey("w")) { boatPos.y += gameSpeed * Time.deltaTime; }
        if (Input.GetKey("s")) { boatPos.y -= gameSpeed * Time.deltaTime; }
        if (Input.GetKey("d")) { boatPos.x += gameSpeed * Time.deltaTime; }
        if (Input.GetKey("a")) { boatPos.x -= gameSpeed * Time.deltaTime; }
        transform.position = boatPos; 


        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        Debug.Log(normalizedBoatPos.x);
    }

    void MoveFromThinking()
    {
        GameObject nearestObstacle = FindNearest(GameObject.FindGameObjectsWithTag("Obstacle"));
        Vector3 boatPos = transform.position;

        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        
        float obstacleXCoord = 0.0f;
        if(nearestObstacle != null){
            Vector3 normalizedObstaclePos = Camera.main.WorldToViewportPoint(nearestObstacle.transform.position);
            obstacleXCoord = normalizedObstaclePos.x;
        }
        
        float[] input = {
            normalizedBoatPos.x,
            obstacleXCoord
        };

        Debug.Log(normalizedBoatPos.x + " " + obstacleXCoord);

        brain.FeedForward(input);
        float[] directions = brain.Outputs();
        
        float biggestInput = 0.0f;
        int indexOfBiggest = 0;
        for(int i=0; i < directions.Length; i++){
            if(directions[i] > biggestInput){
                biggestInput = directions[i];
                indexOfBiggest = i;
            }
        }

        switch (indexOfBiggest) {
            case 0:
                boatPos.y += gameSpeed * Time.deltaTime; // Up
                break;
            case 1:
                boatPos.y -= gameSpeed * Time.deltaTime; // Up
                break;
            case 2:
                boatPos.x -= gameSpeed * Time.deltaTime;
                break;
            case 3:
                boatPos.x += gameSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
        transform.position = boatPos;
    }

    GameObject FindNearest(GameObject[] objects){
        GameObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject gameObject in objects)
        {
            float newDistance = (gameObject.transform.position - position).sqrMagnitude;
            if (newDistance < distance)
            {
                closestObject = gameObject;
                distance = newDistance;
            }
        }
        return closestObject;
    }

    void UpdateScore()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        float heightOnScreen = pos.y;
        float newDistanceTraveled = gameSpeed;
        score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        distanceTraveled += newDistanceTraveled;
    }

    void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
