﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatGame : GameBase, IGame
{
    public GameObject background;

    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public float obstacleSpawnPeriod = 0.25f;
    public bool activeGame = false;

    public event Action OnGameStart = delegate { };
    public event Action OnGameOver = delegate { };


    public void Start()
    {
        Loader.LoaderCallback();
        GamePiece.OnComponentCollision += ManageCollisions;
    }

    public void OnDestroy()
    {
        GamePiece.OnComponentCollision -= ManageCollisions;
    }

    void Update()
    {
        if (!activeGame)
            return;
        for (int i = 0; i < activePlayers.Count; i++)
        {
            Player player = activePlayers[i];
            if (!player.PlayerObject.activeSelf)
            {
                activePlayers.Remove(player);
                inactivePlayers.Add(player);
                i--;
            }
        }
        UpdateScores();
    }

    void LateUpdate()
    {
        if (!activeGame)
            return;
        if (activePlayers.Count <= 0)
        {
            GameOver();
        }
    }


    /*****************
    * Start and End  *
    *****************/
    public void StartGame()
    {
        InvokeRepeating("AddObstacle", 0.25f, obstacleSpawnPeriod);
        OnGameStart();
        activeGame = true;
    }

    public void GameOver()
    {
        CancelInvoke();
        Debug.Log("Calling Game Over");
        OnGameOver();
    }


    /*********************
   * Simulation Methods *
   *********************/
    public IEnumerator StartAllSimulations()
    {
        for (int i = 0; i < simulations.Count; i++)
        {
            Simulation simulation = simulations[i];
            for (int j = 0; j < simulation.playerCount; j++)
            {
                Player newPlayer = AddPlayer(simulation);
                for (int k = i; k >= 0; k--)
                {
                    simulations[k].IgnoreCollisionsWith(newPlayer.PlayerObject);
                }
            }
            GameObject newObstacke = AddObstacle(simulation);
            for (int k = i; k >= 0; k--)
            {
                simulations[k].IgnoreCollisionsWith(newObstacke);
            }
        }
        yield return new WaitForSeconds(1);
    }


    /*****************
    * Player Methods *
    *****************/
    public Player AddPlayer(Simulation simulation, NeuralNet brain = null)
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(UnityEngine.Random.Range(0.3f, 0.7f), 0.3f));
        normalizedPosition.z = 0;
        return AddPlayer(normalizedPosition, simulation, brain);
    }

    public Player AddPlayer(Vector3 normalizedPosition, Simulation simulation, NeuralNet brain = null)
    {
        GameObject playerObject = Instantiate(playerTemplate.prefabObject);
        if (playerObject == null)
            return null;

        playerObject.GetComponent<GamePiece>().template = playerTemplate;
        if (brain != null)
            playerObject.GetComponent<GamePiece>().brain = brain.Clone();
        playerObject.transform.position = normalizedPosition;
        playerObject.layer = playerLayer;
        playerObject.transform.SetParent(playerContainer.transform);
        playerObject.SetActive(true);
        Player player = new Player(playerObject, simulation);
        simulation.AddPlayer(player);
        activePlayers.Add(player);
        return player;
    }

    /*******************
    * Obstacle Methods *
    *******************/
    public GameObject AddObstacle(Simulation simulation)
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(UnityEngine.Random.Range(-0.10f, 1.10f), 1.1f));
        normalizedPosition.z = 0;
        return AddObstacle(normalizedPosition, simulation);
    }

    public GameObject AddObstacle(Vector3 normalizedPosition, Simulation simulation)
    {
        GameObject obstacle = Instantiate(obstacleTemplate.prefabObject);
        obstacle.GetComponent<GamePiece>().template = obstacleTemplate;
        obstacle.layer = obstacleLayer;
        obstacle.transform.SetParent(obstacleContainer.transform);
        if (obstacle != null)
        {
            obstacle.transform.position = normalizedPosition;
            obstacle.SetActive(true);
        }
        simulation.AddObstacle(obstacle);
        return obstacle;
    }

    /*******************
    *       Misc       *
    *******************/
    public void SetTimer(float timer) { }
    
    public void UpdateScores()
    {
        foreach(Player player in activePlayers)
        {
            float newDistanceTraveled = 10.0f;
            player.Score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled)));
        }
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
        if (gameObject.CompareTag("Player") && (collidedObject.CompareTag("Obstacle") || collidedObject.CompareTag("Border")))
        {
            gameObject.SetActive(false);
        }
    }

}
