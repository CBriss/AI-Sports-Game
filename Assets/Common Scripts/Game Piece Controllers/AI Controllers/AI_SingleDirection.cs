using UnityEngine;

[CreateAssetMenu(fileName = "new single direction", menuName = "Game Pieces/Controller/AI Single Direction")]
public class AI_SingleDirection : GamePieceController
{
    public float movementSpeed;
    public Vector2 direction;

    public bool clampToScreen;

    public override void UpdateComponent(GamePiece gamePiece)
    {
        Vector2 pos = gamePiece.transform.position;
        gamePiece.SetPosition(pos + direction * movementSpeed * Time.deltaTime, clampToScreen);
    }
}
