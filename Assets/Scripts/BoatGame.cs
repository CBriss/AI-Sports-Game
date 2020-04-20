using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatGame : MonoBehaviour, IGameManager
{
    public GameObject background;
    public GameObject gameComponentPrefab;
    public GameComponentTemplate playerTemplate;
    public GameComponentTemplate obstacleTemplate;
    public List<Player> activePlayers;
    public List<Player> inactivePlayers;
    public float gameSpeed = 0.5f;

    public void Start()
    {
        activePlayers = new List<Player>();
        inactivePlayers = new List<Player>();
        background = GameObject.Find("background");
        InvokeRepeating("AddObstacles", 0.25f, 0.25f);
    }

    void Update()
    {
        for (int i = 0; i < activePlayers.Count; i++)
        {
            Player player = activePlayers[i];
            if (player.playerObject == null)
            {
                activePlayers.Remove(player);
                inactivePlayers.Add(player);
                i--;
            }
        }
        UpdatePlayers();
    }
    void LateUpdate()
    {
        if (activePlayers.Count <= 0)
            GameOver();
    }

    public void AddPlayer()
    {
        Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newObstacleNormalizedPosition.z = 0;

        GameObject player = Instantiate(gameComponentPrefab);
        player.GetComponent<GameComponent>().template = playerTemplate;
        if (player != null)
        {
            player.transform.position = newObstacleNormalizedPosition;
            player.SetActive(true);
            activePlayers.Add(new Player(player));
        }
    }

    public void AddPlayer(Vector3 newObstacleNormalizedPosition)
    {
        GameObject player = Instantiate(gameComponentPrefab);
        player.GetComponent<GameComponent>().template = playerTemplate;
        if (player != null)
        {
            player.transform.position = newObstacleNormalizedPosition;
            player.SetActive(true);
            activePlayers.Add(new Player(player));
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

    public void SetActivePlayers(List<Player> playerList)
    {
        activePlayers = playerList;
    }

    public void SetInactivePlayers(List<Player> playerList)
    {
        inactivePlayers = playerList;
    }

    void AddObstacles()
    {
        if (Random.Range(-1f, 1f) > 0.25f)
        {
            Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0f, 1f), 1.1f));
            newObstacleNormalizedPosition.z = 0;

            GameObject obstacle = Instantiate(gameComponentPrefab);
            obstacle.GetComponent<GameComponent>().template= obstacleTemplate;
            if (obstacle != null)
            {
                obstacle.transform.position = newObstacleNormalizedPosition;
                obstacle.SetActive(true);
            }
        }
    }

    void UpdatePlayers()
    {
        foreach(Player player in activePlayers)
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(player.playerObject.transform.position);
            float heightOnScreen = pos.y;
            float newDistanceTraveled = gameSpeed;
            player.score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
