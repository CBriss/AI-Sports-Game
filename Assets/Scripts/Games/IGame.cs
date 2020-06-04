using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    event Action OnGameStart;
    event Action OnGameOver;
    void StartGame();
    bool IsActive();
    void SetPlayerTemplate(GamePieceTemplate playerTemplate);
    void SetObstacleTemplate(GamePieceTemplate obstacleTemplate);
    Player AddPlayer(Vector3 normalizedPostion, NeuralNet brain = null);
    void AddObstacle();
    void AddObstacle(Vector3 normalizedPostion);
    List<Player> GetActivePlayers();
    List<Player> GetInactivePlayers();
    void Clear();
    void ClearActivePlayers();
    void ClearInactivePlayers();
    void RemoveFromInactivePlayers(Player player);
    void ClearObstacles();
    void UpdateScores();
    void GameOver();
}
