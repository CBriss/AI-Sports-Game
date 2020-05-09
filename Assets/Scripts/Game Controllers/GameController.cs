using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameController: MonoBehaviour
{
    public abstract void StartGame();
    public abstract void Update();
    public abstract GameComponentTemplate GetPlayerTemplate();
    public abstract GameComponentTemplate GetObstacleTemplate();
}
