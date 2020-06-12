using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    event Action OnGameStart;
    event Action OnGameOver;
    /*****************
    * Start and End  *
    *****************/
    void StartGame();
    void GameOver();

    /*****************
    * Player Methods *
    *****************/
    void SetPlayerTemplate(GamePieceTemplate playerTemplate);
    Player AddPlayer(NeuralNet brain = null);
    Player AddPlayer(Vector3 normalizedPostion, NeuralNet brain = null);
    List<Player> GetActivePlayers();
    void ClearActivePlayers();
    List<Player> GetInactivePlayers();
    void ClearInactivePlayers();
    void RemoveFromInactivePlayers(Player player);

    /*******************
    * Obstacle Methods *
    *******************/
    void SetObstacleTemplate(GamePieceTemplate obstacleTemplate);
    void AddObstacle();
    void AddObstacle(Vector3 normalizedPostion);
    void ClearObstacles();

    /*******************
    *       Misc       *
    *******************/
    void Clear();
    void SetTimer(float timer);
    bool IsActive();
    void UpdateScores();
}
