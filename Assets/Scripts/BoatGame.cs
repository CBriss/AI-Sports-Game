using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatGame : MonoBehaviour
{
    public GameObject background;
    public GameObject gameComponentPrefab;
    public GameComponentTemplate playerTemplate;
    public GameComponentTemplate obstacleTemplate;
    public List<Player> players;
    // public bool playerFlag;
    // public int speedMode = 1;
    public float gameSpeed = 0.5f;
    public GeneticAlgorithm geneticAlgorithm;

    public class Player
    {
        public GameObject playerObject { get; }
        public int score { get; set; }

        public Player(GameObject playerGameObject)
        {
            playerObject = playerGameObject;
            score = 0;
        }
    }

    void Start()
    {
        players = new List<Player>();
        background = GameObject.Find("background");
        InvokeRepeating("AddObstacles", 0.25f, 0.25f);
        AddPlayer();
    }

    void LateUpdate()
    {
        UpdatePlayerScores();
        int numberAlive = 0;
        foreach(Player player in players)
        {
            if (player.playerObject.activeInHierarchy)
                numberAlive++;
        }
        if (numberAlive <= 0)
            GameOver();
    }

    void AddPlayer()
    {
        Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newObstacleNormalizedPosition.z = 0;

        GameObject player = Instantiate(gameComponentPrefab);
        player.GetComponent<GameComponent>().template = playerTemplate;
        if (player != null)
        {
            player.transform.position = newObstacleNormalizedPosition;
            player.SetActive(true);
            players.Add(new Player(player));
        }
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

    void UpdatePlayerScores()
    {
        for(int i=0; i < players.Count; i++)
        {
            Player player = players[i];
            Vector3 pos = Camera.main.WorldToViewportPoint(player.playerObject.transform.position);
            float heightOnScreen = pos.y;
            float newDistanceTraveled = gameSpeed;
            player.score += Mathf.RoundToInt(Mathf.Ceil((newDistanceTraveled) * (heightOnScreen * 1.5f)));
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
