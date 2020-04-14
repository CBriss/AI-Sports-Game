using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    private float gameSpeed;

    void Update()
    {
        MoveFromInput();
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
    /*

    void MoveFromThinking()
    {
        GameObject nearestObstacle = FindNearest(GameObject.FindGameObjectsWithTag("Obstacle"));
        Vector3 boatPos = transform.position;

        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);

        float obstacleXCoord = 0.0f;
        if (nearestObstacle != null)
        {
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
        for (int i = 0; i < directions.Length; i++)
        {
            if (directions[i] > biggestInput)
            {
                biggestInput = directions[i];
                indexOfBiggest = i;
            }
        }

        switch (indexOfBiggest)
        {
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
    */

    void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
