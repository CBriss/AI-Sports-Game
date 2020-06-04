using System;
using System.Collections.Generic;
using UnityEngine;

public class SportGame : MonoBehaviour, IGame
{
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public bool activeGame = false;

    public GameObject playerContainer;
    public GamePieceTemplate playerTemplate;
    public int playerLayer;

    public GameObject obstacleContainer;
    public GamePieceTemplate obstacleTemplate;
    public int obstacleLayer;

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

    public void SetPlayerTemplate(GamePieceTemplate playerTemplate)
    {
        this.playerTemplate = playerTemplate;
    }

    public void SetObstacleTemplate(GamePieceTemplate obstacleTemplate)
    {
        this.obstacleTemplate = obstacleTemplate;
    }

    public void StartGame()
    {
        activeGame = true;
        AddObstacle();
        OnGameStart();
    }

    public bool IsActive()
    {
        return activeGame;
    }

    public void Clear()
    {
        ClearActivePlayers();
        ClearInactivePlayers();
        ClearObstacles();
    }

    void Update()
    {
        if (!activeGame)
            return;
    }

    void LateUpdate()
    {
        if (!activeGame)
            return;
    }

    public Player AddPlayer(Vector3 normalizedPosition, NeuralNet brain = null)
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
        Player player = new Player(playerObject);
        activePlayers.Add(player);
        return player;
    }

    public void AddObstacle()
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(UnityEngine.Random.Range(-0.10f, 1.10f), 1.1f));
        normalizedPosition.z = 0;
        AddObstacle(normalizedPosition);
    }

    public void AddObstacle(Vector3 normalizedPosition)
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
    }

    public List<Player> GetActivePlayers()
    {
        return activePlayers;
    }

    public List<Player> GetInactivePlayers()
    {
        return inactivePlayers;
    }

    public void ClearActivePlayers()
    {
        foreach (Player player in activePlayers)
        {
            Destroy(player.playerObject);
        }

        activePlayers = new List<Player>();
    }

    public void ClearInactivePlayers()
    {
        foreach (Player player in inactivePlayers)
        {
            Destroy(player.playerObject);
        }

        inactivePlayers = new List<Player>();
    }

    public void RemoveFromInactivePlayers(Player player)
    {
        inactivePlayers.Remove(player);
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
    }

    public void UpdateScores()
    {
    }

    public void GameOver()
    {
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
    }

}
