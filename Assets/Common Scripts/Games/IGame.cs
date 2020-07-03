using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame
{
    event Action OnGameStart;
    event Action OnGameOver;

    /****************
    * Start and End *
    ****************/
    void StartGame();
    void GameOver();

    /*********************
    * Simulation Methods *
    *********************/
    Simulation AddSimulation(int playerCount);
    void RemoveSimulation(Simulation simulation);
    List<Simulation> GetSimulations(bool includeActive, bool includeInactive);
    void ClearSimulations(bool active, bool inactive);
    IEnumerator StartAllSimulations();

    /*****************
    * Player Methods *
    *****************/
    void SetPlayerTemplate(GamePieceTemplate playerTemplate);
    Player AddPlayer(Simulation simulation, NeuralNet brain = null);
    Player AddPlayer(Vector3 normalizedPostion, Simulation simulation, NeuralNet brain = null);

    /*******************
    * Obstacle Methods *
    *******************/
    void SetObstacleTemplate(GamePieceTemplate obstacleTemplate);
    GameObject AddObstacle(Simulation simulation);
    GameObject AddObstacle(Vector3 normalizedPostion, Simulation simulation);

    /********
    *  Misc *
    ********/
    void Clear();
    void SetTimer(float timer);
    bool IsActive();
    Simulation AddSimulation();
}
