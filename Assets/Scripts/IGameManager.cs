using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    List<Player> GetActivePlayers();
    List<Player> GetInactivePlayers();
    void SetActivePlayers(List<Player> playerList);
    void SetInactivePlayers(List<Player> playerList);
    void Start();
    void AddPlayer();
    void AddPlayer(Vector3 newObstacleNormalizedPosition);
    void GameOver();
}
