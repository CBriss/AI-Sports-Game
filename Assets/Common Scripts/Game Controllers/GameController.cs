using System;
using UnityEngine;

public abstract class GameController : MonoBehaviour
{
    public abstract void StartGameController();
    public abstract void HandleGameOver();
    public abstract GamePieceTemplate GetPlayerTemplate();
    public abstract GamePieceTemplate GetObstacleTemplate();
}
