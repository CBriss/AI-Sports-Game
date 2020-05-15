using System;
using System.Collections.Generic;
using UnityEngine;

public class BoatGame : MonoBehaviour, IGame
{
    public GameObject background;
    public GameObject gameComponentPrefab;
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public float obstacleSpawnPeriod = 0.25f;
    public int playerLayer;
    public int obstacleLayer;

    public bool activeGame = false;
    public GameObject StartUI;

    public GameController gameController;

    public GameObject playerContainer;
    public GameObject obstacleContainer;

    public event Action OnGameStart = delegate { };
    public event Action OnGameOver = delegate { };

    public void Start()
    {
        Loader.LoaderCallback();
        GameComponent.OnComponentCollision += ManageCollisions;
        OnGameStart();
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void StartGame()
    {
        StartUI.SetActive(false);
        background = GameObject.Find("background");
        InvokeRepeating("AddObstacle", 0.25f, obstacleSpawnPeriod);
        gameController.StartGame();
        activeGame = true;
    }

    public bool IsActive()
    {
        return activeGame;
    }
    public void Clear(){
        ClearActivePlayers();
        ClearInactivePlayers();
        ClearObstacles();
    }
    void Update()
    {
        if (!activeGame)
            return;
        for (int i = 0; i < activePlayers.Count; i++)
        {
            Player player = activePlayers[i];
            if (!player.playerObject.activeSelf)
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
            activeGame = false;
            GameOver();
        }
    }

    public Player AddPlayer(Vector3 normalizedPosition, NeuralNet brain = null)
    {
        GameObject playerObject = Instantiate(gameComponentPrefab);
        if (playerObject == null)
            return null;

        playerObject.GetComponent<GameComponent>().template = gameController.GetPlayerTemplate();
        if (brain != null)
            playerObject.GetComponent<GameComponent>().brain = brain.Clone();
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
        GameObject obstacle = Instantiate(gameComponentPrefab);
        obstacle.GetComponent<GameComponent>().template = gameController.GetObstacleTemplate();
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
        foreach(Player player in activePlayers)
        {
            Destroy(player.playerObject);
        }

        activePlayers = new List<Player>();
    }

    public void ClearInactivePlayers()
    {
        foreach(Player player in inactivePlayers)
        {
            Destroy(player.playerObject);
        }

        inactivePlayers = new List<Player>();
    }

    public void ClearObstacles()
    {
        foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
    }
    
    public void UpdateScores()
    {
        foreach(Player player in activePlayers)
        {
            float newDistanceTraveled = 10.0f;
            player.score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled)));
        }
    }

    public void GameOver()
    {
        OnGameOver();
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        Loader.Load(SceneLoader.Scenes.MainMenuScene.ToString());
        
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
        if (gameObject.tag == "Player" && (collidedObject.gameObject.tag == "Obstacle" || collidedObject.gameObject.tag == "Border"))
        {
            gameObject.SetActive(false);
        }
    }

}
