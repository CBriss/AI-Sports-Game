using UnityEngine;

[CreateAssetMenu(fileName = "new player keys movement", menuName = "Game Pieces/Controller/Player Keys Movement")]
public class Player_BasicMovement_Keys : GamePieceController
{
    [SerializeField] private KeyCode up_key;
    [SerializeField] private KeyCode down_key;
    [SerializeField] private KeyCode left_key;
    [SerializeField] private KeyCode right_key;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool clampToScreen;
    [SerializeField] private bool rotate;

    public override void UpdateComponent(GamePiece gamePiece)
    {
        if (rotate)
        {
            if (Input.GetKey(up_key)) { gamePiece.movement.MoveForward(gamePiece, movementSpeed, clampToScreen); }
            if (Input.GetKey(down_key)) { gamePiece.movement.MoveBackward(gamePiece, movementSpeed, clampToScreen); }
            if (Input.GetKey(left_key)) { gamePiece.movement.Rotate(gamePiece, "LEFT", rotationSpeed); }
            if (Input.GetKey(right_key)) { gamePiece.movement.Rotate(gamePiece, "RIGHT", rotationSpeed); }
        }
        else
        {
            if (Input.GetKey(up_key)) { gamePiece.movement.MoveUp(gamePiece, movementSpeed, clampToScreen); }
            if (Input.GetKey(down_key)) { gamePiece.movement.MoveDown(gamePiece, movementSpeed, clampToScreen); }
            if (Input.GetKey(left_key)) { gamePiece.movement.MoveLeft(gamePiece, movementSpeed, clampToScreen); }
            if (Input.GetKey(right_key)) { gamePiece.movement.MoveRight(gamePiece, movementSpeed, clampToScreen); }
        }

    }
}

