using UnityEngine;

public class GamePieceMovement : MonoBehaviour
{
    Vector2 objectPosition;

    public void Start()
    {
        objectPosition = gameObject.transform.position;
    }

    public void MoveUp(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        objectPosition.y += movementSpeed * Time.deltaTime;
        GamePiece.SetPosition(objectPosition, clampToScreen);
    }

    public void MoveDown(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        objectPosition.y -= movementSpeed * Time.deltaTime;
        GamePiece.SetPosition(objectPosition, clampToScreen);
    }

    public void MoveLeft(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        objectPosition.x -= movementSpeed * Time.deltaTime;
        GamePiece.SetPosition(objectPosition, clampToScreen);
    }

    public void MoveRight(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        objectPosition.x += movementSpeed * Time.deltaTime;
        GamePiece.SetPosition(objectPosition, clampToScreen);
    }

    public void Rotate(GamePiece GamePiece, string direction, float rotationSpeed)
    {
        Vector3 objectRotation = new Vector3(0, 0, 0);
        if (direction.Equals("LEFT")) { objectRotation.z += rotationSpeed * Time.deltaTime; }
        if (direction.Equals("RIGHT")) { objectRotation.z -= rotationSpeed * Time.deltaTime; }
        GamePiece.transform.Rotate(objectRotation);
    }

    public void MoveForward(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        Vector2 objectPosition = GamePiece.transform.position;
        Vector2 objectMovement = new Vector3(0, 0);
        objectMovement.y += movementSpeed * Time.deltaTime; 
        objectMovement = GamePiece.transform.TransformDirection(objectMovement);
        GamePiece.SetPosition(objectPosition + objectMovement, clampToScreen);
    }

    public void MoveBackward(GamePiece GamePiece, float movementSpeed, bool clampToScreen)
    {
        Vector2 objectPosition = GamePiece.transform.position;
        Vector2 objectMovement = new Vector3(0, 0);
        objectMovement.y -= movementSpeed * Time.deltaTime;
        objectMovement = GamePiece.transform.TransformDirection(objectMovement);
        GamePiece.SetPosition(objectPosition + objectMovement, clampToScreen);
    }



}
