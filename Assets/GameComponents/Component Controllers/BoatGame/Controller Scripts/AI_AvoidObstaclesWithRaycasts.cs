using UnityEngine;

[CreateAssetMenu(fileName = "new AI obstable raycast avoiding", menuName = "Game Components/Movement/AI Avoid Obstacles With Raycasts")]
public class AI_AvoidObstaclesWithRaycasts : ComponentController
{
    public float movementSpeed;

    [Range(4, 8)]
    public int numberOfRays;
    readonly Vector2[] vectors = {
        new Vector2(-0.5f, 0.5f),
        new Vector2(0.5f, 0.5f),
        new Vector2(-0.5f, -0.5f),
        new Vector2(0.5f, -0.5f)
    };

    int layerMask;

    public override void UpdateComponent(GameComponent gameComponent)
    {
        Move(gameComponent);
    }

    public void Move(GameComponent gameComponent)
    {
        float[] input = new float[numberOfRays];
        
        layerMask  = 1 << gameComponent.gameObject.layer;
        layerMask = ~layerMask;

        Vector2 size = gameComponent.GetComponent<Renderer>().bounds.size;
        Vector2 objectPosition = gameComponent.transform.position;

        Vector2 topOfObject = new Vector2(objectPosition.x, objectPosition.y + size.y / 2);
        Vector2 bottomOfObject = new Vector2(objectPosition.x, objectPosition.y - size.y / 2);
        Vector2 leftOfObject = new Vector2(objectPosition.x - size.x / 2, objectPosition.y);
        Vector2 rightOfObject = new Vector2(objectPosition.x + size.x / 2, objectPosition.y);

        input[0] = SendRay(topOfObject, Vector2.up, Color.yellow);
        input[1] = SendRay(bottomOfObject, Vector2.down, Color.red);
        input[2] = SendRay(leftOfObject, Vector2.left, Color.blue);
        input[3] = SendRay(rightOfObject, Vector2.right, Color.green);

        for(int i=0; i < numberOfRays; i++)
        {
            if (i <= 2)
                input[i] = SendRay(topOfObject, vectors[i], Color.yellow);
            else
                input[i] = SendRay(bottomOfObject, vectors[i], Color.red);
        }

        gameComponent.GetComponent<GameComponent>().brain.FeedForward(input);
        
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
        gameComponent.SetPosition(objectPosition, true);
    }

    public float SendRay(Vector2 originalPos, Vector2 rayDirection, Color color)
    {
        RaycastHit2D hit = Physics2D.Raycast(originalPos, rayDirection, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            Debug.DrawLine(originalPos, hit.collider.transform.position, color);
            return hit.distance;
        }
        return 0;
    }
}
