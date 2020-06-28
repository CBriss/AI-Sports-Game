using UnityEngine;

[CreateAssetMenu(fileName = "new AI obstable raycast tracking", menuName = "Game Pieces/Controller/AI Track Obstacles With Raycasts")]
public class AI_TrackObstaclesWithRaycasts : GamePieceController
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

    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed;

    public override void UpdateComponent(GamePiece GamePiece)
    {
        Move(GamePiece);
    }

    public void Move(GamePiece GamePiece)
    {
        float[] input = new float[numberOfRays];
        
        layerMask  = 1 << GamePiece.gameObject.layer;
        layerMask = ~layerMask;

        Vector2 size = GamePiece.GetComponent<Renderer>().bounds.size;
        Vector2 objectPosition = GamePiece.transform.position;

        Vector2 topOfObject = new Vector2(objectPosition.x, objectPosition.y + size.y / 2);
        Vector2 bottomOfObject = new Vector2(objectPosition.x, objectPosition.y - size.y / 2);
        Vector2 leftOfObject = new Vector2(objectPosition.x - size.x / 2, objectPosition.y);
        Vector2 rightOfObject = new Vector2(objectPosition.x + size.x / 2, objectPosition.y);

        input[0] = Rays.SendRay(topOfObject, Vector2.up, Color.yellow, layerMask);
        input[1] = Rays.SendRay(bottomOfObject, Vector2.down, Color.red, layerMask);
        input[2] = Rays.SendRay(leftOfObject, Vector2.left, Color.blue, layerMask);
        input[3] = Rays.SendRay(rightOfObject, Vector2.right, Color.green, layerMask);

        for(int i=0; i < numberOfRays; i++)
        {
            if (i <= 2)
                input[i] = Rays.SendRay(topOfObject, vectors[i], Color.yellow, layerMask);
            else
                input[i] = Rays.SendRay(bottomOfObject, vectors[i], Color.red, layerMask);
        }

        GamePiece.GetComponent<GamePiece>().brain.FeedForward(input);
        
        float[] directions = GamePiece.GetComponent<GamePiece>().brain.Outputs();


        Vector3 objectRotation = new Vector3(0, 0, 0);
        Vector2 objectMovement = new Vector3(0, 0);

        switch (ChooseDirection(directions))
        {
            case 0:
                objectPosition.y += movementSpeed * Time.deltaTime;
                break;
            case 1:
                objectPosition.y -= movementSpeed * Time.deltaTime;
                break;
            case 2:
                if(rotate)
                    objectRotation.z += rotationSpeed * Time.deltaTime;
                else
                    objectPosition.x -= movementSpeed * Time.deltaTime;
                break;
            case 3:
                if(rotate)
                    objectRotation.z -= rotationSpeed * Time.deltaTime;
                else
                    objectPosition.x += movementSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
        objectMovement = GamePiece.transform.TransformDirection(objectMovement);
        GamePiece.SetPosition(objectPosition + objectMovement, true);
        GamePiece.transform.Rotate(objectRotation);
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
