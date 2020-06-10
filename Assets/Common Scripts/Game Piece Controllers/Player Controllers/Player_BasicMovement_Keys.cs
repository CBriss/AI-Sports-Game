using UnityEngine;

[CreateAssetMenu(fileName = "new player keys movement", menuName = "Game Pieces/Controller/Player Keys Movement")]
public class Player_BasicMovement_Keys : GamePieceController
{
    [SerializeField]
    private KeyCode up_key;
    [SerializeField]
    private KeyCode down_key;
    [SerializeField]
    private KeyCode left_key;
    [SerializeField]
    private KeyCode right_key;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool clampToScreen;

    public override void UpdateComponent(GamePiece GamePiece)
    {
        Move(GamePiece);
    }
    public void Move(GamePiece GamePiece)
    {
        Vector2 objectPosition = GamePiece.transform.position;
        if (Input.GetKey(up_key)) { objectPosition.y += movementSpeed * Time.deltaTime; }
        if (Input.GetKey(down_key)) { objectPosition.y -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(left_key)) { objectPosition.x -= movementSpeed * Time.deltaTime; }
        if (Input.GetKey(right_key)) { objectPosition.x += movementSpeed * Time.deltaTime; }

        GamePiece.SetPosition(objectPosition, clampToScreen);
    }
}
