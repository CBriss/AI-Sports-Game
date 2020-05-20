using System;
using UnityEngine;

public abstract class GameController : MonoBehaviour
{
    public abstract void StartGameController();
    public abstract void HandleGameOver();
    public abstract GameComponentTemplate GetPlayerTemplate();
    public abstract GameComponentTemplate GetObstacleTemplate();
}
