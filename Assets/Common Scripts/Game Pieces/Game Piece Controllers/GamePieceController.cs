using UnityEngine;

public abstract class GamePieceController : ScriptableObject
{
    public abstract void UpdateComponent(GamePiece gameComponent);
}
