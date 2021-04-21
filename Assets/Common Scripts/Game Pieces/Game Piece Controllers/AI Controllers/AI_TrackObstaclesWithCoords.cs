using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new AI obstable coord tracking", menuName = "Game Pieces/Controller/AI Track Obstacles With Coord")]
public class AI_TrackObstaclesWithCoords : GamePieceController
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private bool rotate;
    [SerializeField]
    private int nearestObstaclesCount = 2;
    [SerializeField]
    private bool clampToScreen;

    public override void UpdateComponent(GamePiece GamePiece)
    {
        Move(GamePiece);
    }

    public void Move(GamePiece gamePiece)
    {
        if (nearestObstaclesCount > 0)
        {
            GameObject[] nearestObstacles = FindNearestObstacles(gamePiece, gamePiece.player.simulation.GetObstacles(), nearestObstaclesCount);
            float[] input = GenerateBrainInput(gamePiece, nearestObstacles);
            gamePiece.GetComponent<GamePiece>().brain.FeedForward(input);
        }

        switch (ChooseDirection(gamePiece.GetComponent<GamePiece>().brain.Outputs()))
        {
            case 0:
                gamePiece.movement.MoveForward(gamePiece, movementSpeed, clampToScreen);
                break;
            case 1:
                gamePiece.movement.MoveBackward(gamePiece, movementSpeed, clampToScreen);
                break;
            case 2:
                if (rotate)
                    gamePiece.movement.Rotate(gamePiece, "LEFT", rotationSpeed);
                else
                    gamePiece.movement.MoveLeft(gamePiece, movementSpeed, clampToScreen);
                break;
            case 3:
                if (rotate)
                    gamePiece.movement.Rotate(gamePiece, "RIGHT", rotationSpeed);
                else
                    gamePiece.movement.MoveRight(gamePiece, movementSpeed, clampToScreen);
                break;
            default:
                break;
        }
    }

    public GameObject[] FindNearestObstacles(GamePiece gameObject, List<GameObject> objects, int n)
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

    private float[] GenerateBrainInput(GamePiece gamePiece, GameObject[] nearestObstacles)
    {
        Vector2 objectPosition = gamePiece.transform.position;
        Vector2 normalizedPosition = Camera.main.WorldToViewportPoint(objectPosition);
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
        return input;
    }

    private int ChooseDirection(float[] directions)
    {
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
        return indexOfBiggest;
    }
}
