using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    Player AddPlayer(Vector3 normalizedPostion);
    Player AddPlayer(Vector3 normalizedPostion, NeuralNet brain);
    void AddObstacle();
    void AddObstacle(Vector3 normalizedPostion);
    List<Player> GetActivePlayers();
    List<Player> GetInactivePlayers();
    void ClearActivePlayers();
    void ClearInactivePlayers();
    void UpdateScores();
    void GameOver();
}
