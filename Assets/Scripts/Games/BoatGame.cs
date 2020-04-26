using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatGame : MonoBehaviour, IGame
{
    public GameObject background;
    public GameObject gameComponentPrefab;
    public GameComponentTemplate playerTemplate;
    public GameComponentTemplate obstacleTemplate;
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public float obstacleSpawnPeriod = 0.25f;

    public void Start()
    {
        background = GameObject.Find("background");
        InvokeRepeating("AddObstacle", 0.25f, obstacleSpawnPeriod);
    }

    void Update()
    {
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
        if (activePlayers.Count <= 0)
            GameOver();
    }

    public Player AddPlayer(Vector3 normalizedPosition)
    {
        GameObject playerObject = Instantiate(gameComponentPrefab);
        playerObject.GetComponent<GameComponent>().template = playerTemplate;
        if (playerObject != null)
        {
            playerObject.transform.position = normalizedPosition;
            playerObject.SetActive(true);
            Player player = new Player(playerObject);
            activePlayers.Add(player);
            return player;
        }
        return null;
    }

    public Player AddPlayer(Vector3 normalizedPosition, NeuralNet brain)
    {
        GameObject playerObject = Instantiate(gameComponentPrefab);
        playerObject.GetComponent<GameComponent>().template = playerTemplate;
        playerObject.GetComponent<GameComponent>().brain = brain.Clone();
        if (playerObject != null)
        {
            playerObject.transform.position = normalizedPosition;
            playerObject.SetActive(true);
            Player player = new Player(playerObject);
            activePlayers.Add(player);
            return player;
        }
        return null;
    }

    public void AddObstacle()
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(-0.10f, 1.10f), 1.1f));
        normalizedPosition.z = 0;
        AddObstacle(normalizedPosition);
    }

    public void AddObstacle(Vector3 normalizedPosition)
    {
        GameObject obstacle = Instantiate(gameComponentPrefab);
        obstacle.GetComponent<GameComponent>().template = obstacleTemplate;
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
        activePlayers = new List<Player>();
    }

    public void ClearInactivePlayers()
    {
        inactivePlayers = new List<Player>();
    }
    
    public void UpdateScores()
    {
        foreach(Player player in activePlayers)
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(player.playerObject.transform.position);
            float heightOnScreen = pos.y;
            float newDistanceTraveled = 10.0f;
            player.score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        }
    }

    public void GameOver()
    {
        foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        SceneManager.LoadScene("MenuScene");
    }

}
