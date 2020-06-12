 using System;
using System.Collections.Generic;
using UnityEngine;

public class SportGame : MonoBehaviour, IGame
{
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public List<Player> team_1 = new List<Player>();
    public List<Player> team_2 = new List<Player>();

    public GameObject ball;

    public bool activeGame = false;
    public bool multipleGames = false;
    public bool timedGame = false;
    public float gameTime;
    public float currentTimer;

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

    void Update()
    {
        if (!activeGame)
            return;
    }

    void LateUpdate()
    {
        if (activeGame && timedGame)
        {
            if (currentTimer > 0)
                currentTimer -= Time.deltaTime;
            else if (currentTimer <= 0)
                GameOver();
            Debug.Log("Timer: " + Mathf.Round(currentTimer));
        }
    }

    /*****************
    * Start and End  *
    *****************/
    public void StartGame()
    {
        AddObstacle();
        OnGameStart();
        activeGame = true;
    }

    public void GameOver()
    {
        ClearObstacles();
        for (int i = 0; i < activePlayers.Count; i++)
        {
            Player player = activePlayers[i];
            activePlayers.Remove(player);
            inactivePlayers.Add(player);
            i--;
        }
        ClearActivePlayers();
        Debug.Log("Calling Game Over");

        currentTimer = gameTime;
        OnGameOver();
    }

    /*****************
    * Player Methods *
    *****************/
    public void SetPlayerTemplate(GamePieceTemplate playerTemplate)
    {
        this.playerTemplate = playerTemplate;
    }

    public Player AddPlayer(NeuralNet brain = null)
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(UnityEngine.Random.Range(0.3f, 0.7f), 0.3f));
        normalizedPosition.z = 0;
        return AddPlayer(normalizedPosition);
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
        team_1.Add(player);
        return player;
    }

    public List<Player> GetActivePlayers()
    {
        return activePlayers;
    }

    public void ClearActivePlayers()
    {
        foreach (Player player in activePlayers)
        {
            Destroy(player.playerObject);
        }
        activePlayers = new List<Player>();
        team_1 = new List<Player>();
    }

    public List<Player> GetInactivePlayers()
    {
        return inactivePlayers;
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
        team_1.Remove(player);
    }

    /*******************
    * Obstacle Methods *
    *******************/
    public void SetObstacleTemplate(GamePieceTemplate obstacleTemplate)
    {
        this.obstacleTemplate = obstacleTemplate;
    }
    public void AddObstacle()
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
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
        ball = obstacle;
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
    }


    /*******************
    *       Misc       *
    *******************/
    public void Clear()
    {
        ClearActivePlayers();
        ClearInactivePlayers();
        ClearObstacles();
    }

    public void SetTimer(float timer)
    {
        timedGame = true;
        gameTime = timer;
    }

    public bool IsActive()
    {
        return activeGame;
    }

    public void UpdateScores()
    {
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
        if (gameObject.GetInstanceID() == ball.GetInstanceID() && collidedObject.gameObject.CompareTag("Goal"))
        {
            UpdateScores();
            GameOver();
        }
    }

}
