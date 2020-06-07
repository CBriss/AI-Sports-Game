using UnityEngine;

[CreateAssetMenu(fileName = "new single direction", menuName = "Game Pieces/Controller/AI Single Direction")]
public class AI_SingleDirection : GamePieceController
{
    public float movementSpeed;
    public Vector2 direction;

    public bool clampToScreen;

    public override void UpdateComponent(GamePiece GamePiece)
    {
        Move(GamePiece);
    }
    public void Move(GamePiece GamePiece)
    {
        Vector2 pos = GamePiece.transform.position;
        GamePiece.SetPosition(pos + direction * movementSpeed * Time.deltaTime, clampToScreen);
    }
}
