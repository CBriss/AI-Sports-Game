using UnityEngine;

[CreateAssetMenu(fileName = "new AI obstable raycast avoiding", menuName = "Game Components/Movement/AI Avoid Obstacles With Raycasts")]
public class AI_AvoidObstaclesWithRaycasts : ComponentController
{
    public float movementSpeed;

    public override void UpdateComponent(GameComponent gameComponent)
    {
        Move(gameComponent);
    }
    public void Move(GameComponent gameComponent)
    {
        Vector2 objectPosition = gameComponent.transform.position;
        Vector2 topOfObject = new Vector2(objectPosition.x, objectPosition.y + gameComponent.GetComponent<Renderer>().bounds.size.y / 2);
        Vector2 bottomOfObject = new Vector2(objectPosition.x, objectPosition.y - gameComponent.GetComponent<Renderer>().bounds.size.y / 2);

        int layerMask = 1 << gameComponent.gameObject.layer;
        layerMask = ~layerMask;
        Vector2[] vectors = {Vector2.up, new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f)};
        Color[] vectorColors = {Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.magenta};

        float[] input = new float[(vectors.Length * 2)];
        //float[] input = new float[(vectors.Length * 2) + 2];
        //input[0]=topOfObject.x;
        //input[1]=topOfObject.y;

        for (int i = 0; i < vectors.Length; i++){
            RaycastHit2D hit = Physics2D.Raycast(topOfObject, vectors[i], Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Debug.DrawLine(topOfObject, hit.collider.transform.position, vectorColors[i]);

                input[i] = hit.distance;
                //input[i+2] = hit.distance;
            }

            hit = Physics2D.Raycast(bottomOfObject, vectors[i]*-1, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Debug.DrawLine(bottomOfObject, hit.collider.transform.position, vectorColors[i*2]);

                input[i*2] = hit.distance;
                //input[(i*2) + 2] = hit.distance;
            }
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
}
