using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoatGame : MonoBehaviour, IGame
{
    public GameObject background;
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public float obstacleSpawnPeriod = 0.25f;
    public bool activeGame = false;

    public GameObject playerContainer;
    public GameComponentTemplate playerTemplate;
    public int playerLayer;

    public GameObject obstacleContainer;
    public GameComponentTemplate obstacleTemplate;
    public int obstacleLayer;

    public event Action OnGameStart = delegate { };
    public event Action OnGameOver = delegate { };

    public void Start()
    {
        Loader.LoaderCallback();
        GameComponent.OnComponentCollision += ManageCollisions;
    }

    public void OnDestroy()
    {
        GameComponent.OnComponentCollision -= ManageCollisions;
    }

    public void SetPlayerTemplate(GameComponentTemplate playerTemplate)
    {
        this.playerTemplate = playerTemplate;
    }

    public void SetObstacleTemplate(GameComponentTemplate obstacleTemplate)
    {
        this.obstacleTemplate = obstacleTemplate;
    }

    public void StartGame()
    {
        background = GameObject.Find("background");
        InvokeRepeating("AddObstacle", 0.25f, obstacleSpawnPeriod);
        activeGame = true;
        OnGameStart();
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
            GameOver();
        }
    }

    public Player AddPlayer(Vector3 normalizedPosition, NeuralNet brain = null)
    {
        GameObject playerObject = Instantiate(playerTemplate.prefabObject);
        if (playerObject == null)
            return null;

        playerObject.GetComponent<GameComponent>().template = playerTemplate;
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
        GameObject obstacle = Instantiate(obstacleTemplate.prefabObject);
        obstacle.GetComponent<GameComponent>().template = obstacleTemplate;
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

    public void RemoveFromInactivePlayers(Player player)
    {
        inactivePlayers.Remove(player);
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
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        Debug.Log("Calling Game Over");
        OnGameOver();
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
        if (gameObject.CompareTag("Player") && (collidedObject.gameObject.CompareTag("Obstacle") || collidedObject.gameObject.CompareTag("Border")))
        {
            gameObject.SetActive(false);
        }
    }

}
