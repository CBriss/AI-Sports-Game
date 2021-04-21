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
    [SerializeField] private bool clampToScreen;

    public override void UpdateComponent(GamePiece gamePiece)
    {
        float[] input = DrawRays(gamePiece);
        gamePiece.GetComponent<GamePiece>().brain.FeedForward(input);
        float[] directions = gamePiece.GetComponent<GamePiece>().brain.Outputs();
        Move(gamePiece, directions);
    }

    public void Move(GamePiece gamePiece, float[] directions)
    {
        switch (ChooseDirection(directions))
        {
            case 0:
                gamePiece.movement.MoveForward(gamePiece, movementSpeed, clampToScreen);
                break;
            case 1:
                gamePiece.movement.MoveBackward(gamePiece, movementSpeed, clampToScreen);
                break;
            case 2:
                if(rotate)
                    gamePiece.movement.Rotate(gamePiece, "LEFT", rotationSpeed);
                else
                    gamePiece.movement.MoveLeft(gamePiece, movementSpeed, clampToScreen);
                break;
            case 3:
                if(rotate)
                    gamePiece.movement.Rotate(gamePiece, "RIGHT", rotationSpeed);
                else
                    gamePiece.movement.MoveRight(gamePiece, movementSpeed, clampToScreen);
                break;
            default:
                break;
        }
    }

    private float[] DrawRays(GamePiece gamePiece)
    {
        float[] input = new float[numberOfRays];

        layerMask = 1 << gamePiece.gameObject.layer;
        layerMask = ~layerMask;

        Vector2 size = gamePiece.GetComponent<Renderer>().bounds.size;
        Vector2 objectPosition = gamePiece.transform.position;

        Vector2 topOfObject = new Vector2(objectPosition.x, objectPosition.y + size.y / 2);
        Vector2 bottomOfObject = new Vector2(objectPosition.x, objectPosition.y - size.y / 2);
        Vector2 leftOfObject = new Vector2(objectPosition.x - size.x / 2, objectPosition.y);
        Vector2 rightOfObject = new Vector2(objectPosition.x + size.x / 2, objectPosition.y);

        input[0] = Rays.SendRay(topOfObject, Vector2.up, Color.yellow, layerMask);
        input[1] = Rays.SendRay(bottomOfObject, Vector2.down, Color.red, layerMask);
        input[2] = Rays.SendRay(leftOfObject, Vector2.left, Color.blue, layerMask);
        input[3] = Rays.SendRay(rightOfObject, Vector2.right, Color.green, layerMask);

        for (int i = 0; i < numberOfRays; i++)
        {
            if (i <= 2)
                input[i] = Rays.SendRay(topOfObject, vectors[i], Color.yellow, layerMask);
            else
                input[i] = Rays.SendRay(bottomOfObject, vectors[i], Color.red, layerMask);
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
