using UnityEngine;

[CreateAssetMenu(fileName = "new AI obstable coord avoiding", menuName = "Game Components/Movement/AI Avoid Obstacles With Coord")]
public class AI_AvoidObstaclesWithCoords : ComponentController
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private int nearestObstaclesCount = 2;
    [SerializeField]
    private bool clampToScreen;

    public override void UpdateComponent(GameComponent gameComponent)
    {
        Move(gameComponent);
    }

    public void Move(GameComponent gameComponent)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        Vector2 objectPosition = gameComponent.transform.position;
        Vector2 normalizedPosition = Camera.main.WorldToViewportPoint(objectPosition);

        if (nearestObstaclesCount > 0)
        {
            GameObject[] nearestObstacles = FindNearest(gameComponent, obstacles, nearestObstaclesCount);
            float[] input = new float[2 + (2 * nearestObstaclesCount)];
            input[0] = normalizedPosition.x;
            input[1] = normalizedPosition.y;
            for (int i = 0; i < nearestObstaclesCount; i++)
            {
                GameObject nearestObstacle = nearestObstacles[i];
                Vector2 normalizedObstaclePos;
                if (nearestObstacle != null && nearestObstacles.Length > 0)
                {
                    normalizedObstaclePos = Camera.main.WorldToViewportPoint(nearestObstacle.transform.position);

                    input[2 + i] = normalizedObstaclePos.x;
                    input[2 + i + 1] = normalizedObstaclePos.y;
                }
                else
                {
                    normalizedObstaclePos = new Vector3(0.0f, 0.0f, 0.0f);
                    input[2 + i] = normalizedObstaclePos.x;
                    input[2 + i + 1] = normalizedObstaclePos.y;
                }
            }
            gameComponent.GetComponent<GameComponent>().brain.FeedForward(input);
        }

        float[] directions = gameComponent.GetComponent<GameComponent>().brain.Outputs();

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
                objectPosition.y += movementSpeed * Time.deltaTime;
                break;
            case 1:
                objectPosition.y -= movementSpeed * Time.deltaTime;
                break;
            case 2:
                objectPosition.x -= movementSpeed * Time.deltaTime;
                break;
            case 3:
                objectPosition.x += movementSpeed * Time.deltaTime;
                break;
            default:
                break;
        }

        gameComponent.SetPosition(objectPosition, clampToScreen);
    }


    public GameObject[] FindNearest(GameComponent gameObject, GameObject[] objects, int n)
    {
        GameObject[] closestObjects = new GameObject[n];
        for (int i = 0; i < n; i++)
        {
            GameObject closestObject = null;
            float distance = Mathf.Infinity;
            Vector3 position = gameObject.transform.position;
            foreach (GameObject currentObject in objects)
            {
                float newDistance = (currentObject.transform.position - position).sqrMagnitude;
                if (newDistance < distance)
                {
                    bool alreadyAdded = false;
                    if (closestObjects != null && closestObjects.Length > 0)
                    {
                        foreach (GameObject obstacle in closestObjects)
                        {
                            if (obstacle == currentObject)
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }
                    }
                    if (!alreadyAdded)
                    {
                        closestObject = currentObject;
                        distance = newDistance;
                    }

                }
            }
            closestObjects[i] = closestObject;
        }

        return closestObjects;
    }



}
