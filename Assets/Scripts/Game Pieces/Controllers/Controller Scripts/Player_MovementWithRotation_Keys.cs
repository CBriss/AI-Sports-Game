using UnityEngine;

[CreateAssetMenu(fileName = "new player keys movement", menuName = "Game Pieces/Controller/Player Rorational Keys")]
public class Player_MovementWithRotation_Keys: GamePieceController
{
    [SerializeField] private KeyCode up_key;
    [SerializeField] private KeyCode down_key;
    [SerializeField] private KeyCode left_key;
    [SerializeField] private KeyCode right_key;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool clampToScreen;

    public override void UpdateComponent(GamePiece GamePiece)
    {
        Move(GamePiece);
    }
    public void Move(GamePiece GamePiece)
    {
        Vector2 objectPosition = GamePiece.transform.position;
        Vector3 objectRotation = new Vector3(0, 0, 0);
        Vector2 objectMovement = new Vector3(0, 0);

        if (Input.GetKey(up_key)) { objectMovement.y += movementSpeed * Time.deltaTime; }
        if (Input.GetKey(down_key)) { objectMovement.y -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(left_key)) { objectRotation.z += rotationSpeed * Time.deltaTime; }
        if (Input.GetKey(right_key)) { objectRotation.z -= rotationSpeed * Time.deltaTime; }

        objectMovement = GamePiece.transform.TransformDirection(objectMovement);
        GamePiece.SetPosition(objectPosition + objectMovement, clampToScreen);
        GamePiece.transform.Rotate(objectRotation);
    }
}
