using UnityEngine;

[CreateAssetMenu(fileName = "new AI movement", menuName = "Game Components/AI Movement")]
public class AIMovement : MovementController
{
    public float movementSpeed;
    public int nearestObstaclesCount = 2;
    public override void Move(GameObject gameObject)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        Vector3 boatPos = gameObject.transform.position;
        Vector3 normalizedBoatPos = Camera.main.WorldToViewportPoint(boatPos);
        
        if(nearestObstaclesCount > 0) {
            GameObject[] nearestObstacles = FindNearest(gameObject, obstacles, nearestObstaclesCount);
            float[] input = new float[2+(2*nearestObstaclesCount)];
            input[0] = normalizedBoatPos.x;
            input[1] = normalizedBoatPos.y;
            for(int i=0; i<nearestObstaclesCount; i++) {
                GameObject nearestObstacle = nearestObstacles[i];
                Vector3 normalizedObstaclePos;
                if(nearestObstacle != null && nearestObstacles.Length > 0){
                    normalizedObstaclePos = Camera.main.WorldToViewportPoint(nearestObstacle.transform.position);
                    
                    input[2+i] = normalizedObstaclePos.x;
                    input[2+i+1] = normalizedObstaclePos.y;
                } else {
                    normalizedObstaclePos = new Vector3(0.0f, 0.0f, 0.0f);
                    input[2+i] = normalizedObstaclePos.x;
                    input[2+i+1] = normalizedObstaclePos.y;
                }
            }
            gameObject.GetComponent<GameComponent>().brain.FeedForward(input);
        } else {
            
            Vector2 topOfObject = new Vector2(boatPos.x, boatPos.y + gameObject.GetComponent<Renderer>().bounds.size.y / 2);

            int layerMask = 1 << gameObject.layer;
            layerMask = ~layerMask;
            //Vector2[] vectors = {Vector2.up, new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-1f, 0.0f), new Vector2(1f, 0.0f) };
            Vector2[] vectors = {Vector2.up, new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f)};
            //Vector2[] vectors = {Vector2.up, new Vector2(-1f, 0.0f), new Vector2(1f, 0.0f) };
            Color[] vectorColors = {Color.red, Color.blue, Color.green, Color.yellow, Color.white};
            float[] hitDistances = new float[vectors.Length];
            float[] input = new float[vectors.Length+2];
            input[0]=topOfObject.x;
            input[1]=topOfObject.x;
            
            for(int i = 0; i < vectors.Length; i++){
                RaycastHit2D hit = Physics2D.Raycast(topOfObject, vectors[i], Mathf.Infinity, layerMask);
                if (hit.collider != null)
                {
                    Debug.DrawLine(topOfObject, hit.collider.transform.position, vectorColors[i]);
                    input[i+2] = hit.distance;
                }
            }
            
            // Debug.Log(input[0] + ", " + input[1] + ", " + input[2] + ", " + input[3] + ", " + input[4]);
            gameObject.GetComponent<GameComponent>().brain.FeedForward(input);
        }
        
        
       

        
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
                boatPos.y += movementSpeed * Time.deltaTime;
                break;
            case 1:
                boatPos.y -= movementSpeed * Time.deltaTime;
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


    public GameObject[] FindNearest(GameObject gameObject, GameObject[] objects, int n)
    {
        GameObject[] closestObjects = new GameObject[n];
        for(int i=0;i<n;i++){
            GameObject closestObject = null;
            float distance = Mathf.Infinity;
            Vector3 position = gameObject.transform.position;
            foreach (GameObject currentObject in objects)
            {
                float newDistance = (currentObject.transform.position - position).sqrMagnitude;
                if (newDistance < distance)
                {
                    bool alreadyAdded = false;
                    if (closestObjects!=null && closestObjects.Length>0) {
                        foreach(GameObject obstacle in closestObjects){
                            if (obstacle == currentObject){
                                alreadyAdded = true;
                                break;
                            }
                        }
                    }
                    if (!alreadyAdded){
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
