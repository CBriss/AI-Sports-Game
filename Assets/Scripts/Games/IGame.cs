using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    static event Action<IGame> OnGameStart;
    static event Action<IGame> OnGameOver;

    void StartGame();
    bool IsActive();
    void SetGameController(GameController gameController);
    Player AddPlayer(Vector3 normalizedPostion, NeuralNet brain = null);
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
