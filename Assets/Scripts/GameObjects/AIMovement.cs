using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new AI movement", menuName = "Game Components/AI Movement")]
public class AIMovement : MovementAbility
{
    public float movementSpeed;
    public override void Move(GameObject gameObject)
    {
        GameObject nearestObstacle = FindNearest(gameObject, GameObject.FindGameObjectsWithTag("Obstacle"));
        Vector3 boatPos = gameObject.transform.position;

        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        Vector3 normalizedObstaclePos = Camera.main.WorldToViewportPoint(nearestObstacle.transform.position);
        float[] input = {
            normalizedBoatPos.x / Camera.main.pixelRect.width,
            normalizedObstaclePos.x / Camera.main.pixelRect.width
        };

        gameObject.GetComponent<GameComponent>().brain.FeedForward(input);
        float[] directions = gameObject.GetComponent<GameComponent>().brain.Outputs();

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
                boatPos.y += movementSpeed * Time.deltaTime; // Up
                break;
            case 1:
                boatPos.y -= movementSpeed * Time.deltaTime; // Up
                break;
            case 2:
                boatPos.x -= movementSpeed * Time.deltaTime;
                break;
            case 3:
                boatPos.x += movementSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
        gameObject.transform.position = boatPos;
    }


    public GameObject FindNearest(GameObject gameObject, GameObject[] objects)
    {
        GameObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 position = gameObject.transform.position;
        foreach (GameObject currentObject in objects)
        {
            float newDistance = (currentObject.transform.position - position).sqrMagnitude;
            if (newDistance < distance)
            {
                closestObject = currentObject;
                distance = newDistance;
            }
        }
        return closestObject;
    }
}
