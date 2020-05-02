using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    void StartGame();
    bool IsActive();
    Player AddPlayer(Vector3 normalizedPostion);
    Player AddPlayer(Vector3 normalizedPostion, NeuralNet brain);
    void AddObstacle();
    void AddObstacle(Vector3 normalizedPostion);
    List<Player> GetActivePlayers();
    List<Player> GetInactivePlayers();
    void Clear();
    void ClearActivePlayers();
    void ClearInactivePlayers();
    void ClearObstacles();
    void UpdateScores();
    void GameOver();
}
