using UnityEngine;

[CreateAssetMenu(fileName = "new AI movement", menuName = "Game Components/AI Movement")]
public class AIMovement : MovementAbility
{
    public float movementSpeed;
    public override void Move(GameObject gameObject)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject nearestObstacle = FindNearest(gameObject, obstacles);
        Vector3 boatPos = gameObject.transform.position;

        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        Vector3 normalizedObstaclePos;
        if (nearestObstacle == null)
            normalizedObstaclePos = new Vector3(0.0f, 0.0f, 0.0f);
        else
            normalizedObstaclePos = Camera.main.WorldToViewportPoint(nearestObstacle.transform.position);

        float[] input = {
            normalizedBoatPos.x,
            normalizedBoatPos.y,
            normalizedObstaclePos.x,
            normalizedObstaclePos.y
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

        normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        normalizedBoatPos.x = Mathf.Clamp01(normalizedBoatPos.x);
        normalizedBoatPos.y = Mathf.Clamp01(normalizedBoatPos.y);
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(normalizedBoatPos);
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
