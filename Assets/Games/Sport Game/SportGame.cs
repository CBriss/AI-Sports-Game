 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportGame : GameBase, IGame
{
    public Dictionary<GameObject, Player> pushers = new Dictionary<GameObject, Player>();
    public Dictionary<GameObject, Simulation> balls = new Dictionary<GameObject, Simulation>();

    public bool timedGame = false;
    public float gameTime;
    public float currentTimer;

    public event Action OnGameStart = delegate { };
    public event Action OnGameOver = delegate { };

    private float fixedDeltaTime;
    public float timeScale = 1f;

    public void Start()
    {
        Loader.LoaderCallback();
        GamePiece.OnComponentCollision += ManageCollisions;
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void OnDestroy()
    {
        GamePiece.OnComponentCollision -= ManageCollisions;
    }

    void Update()
    {
        if (!active)
            return;

        Time.timeScale = timeScale;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }

    void LateUpdate()
    {
        if (!active)
            return;

        if (timedGame)
        {
            if (currentTimer > 0)
                currentTimer -= Time.deltaTime;
            else if (currentTimer <= 0)
            {
                GameOver();
            }
        }

        foreach(Simulation simulation in simulations)
        {
            if (simulation.active)
            {
                return;
            }
        }
        GameOver();

        //lastBallPosition = ball.transform.position;
    }

    /*****************
    * Start and End  *
    *****************/
    public void StartGame()
    {
        active = true;

        if (timedGame)
            currentTimer = gameTime;

        OnGameStart();
    }

    public void GameOver()
    {
        foreach (Simulation simulation in simulations)
        {
            if (simulation.active)
            {
                simulation.SetAllPlayersInactive();
                simulation.active = false;
            }
        }
        currentTimer = gameTime;
        Debug.Log("Game Over");
        active = false;
        OnGameOver();
    }


    /*********************
    * Simulation Methods *
    *********************/
    public IEnumerator StartAllSimulations()
    {
        Player lastPlayerAdded = null;
        for (int i=0; i < simulations.Count; i++)
        {
            Simulation simulation = simulations[i];
            for(int j=0; j < simulation.playerCount; j++)
            {
                lastPlayerAdded = AddPlayer(simulation);
            }
            AddObstacle(simulation);
        }

        yield return new WaitUntil(() => lastPlayerAdded.PlayerObject.GetComponent<Collider2D>());

        foreach (Simulation simulation in simulations)
        {
            foreach (Simulation otherSimulation in simulations)
            {
                if (otherSimulation != simulation)
                {
                    foreach (Player player in simulation.GetActivePlayers())
                    {
                        otherSimulation.IgnoreCollisionsWith(player.PlayerObject);
                    }

                    foreach (GameObject obstacle in simulation.GetObstacles())
                    {
                        otherSimulation.IgnoreCollisionsWith(obstacle);
                    }
                }
            }
        }
    }

    /*****************
    * Player Methods *
    *****************/
    public Player AddPlayer(Simulation simulation, NeuralNet brain = null)
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(UnityEngine.Random.Range(0.2f, 0.8f), UnityEngine.Random.Range(0.2f, 0.8f)));
        normalizedPosition.z = 0;
        return AddPlayer(normalizedPosition, simulation);
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
        pushers[player.PlayerObject] = player;
        return player;
    }

    /*******************
    * Obstacle Methods *
    *******************/
    public GameObject AddObstacle(Simulation simulation)
    {
        Vector3 normalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
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
        balls[obstacle] = simulation;
        simulation.AddObstacle(obstacle);

        return obstacle;
    }


    /*******************
    *       Misc       *
    *******************/
    public void SetTimer(float timer)
    {
        timedGame = true;
        gameTime = timer;
    }

    private void ManageCollisions(GameObject gameObject, GameObject collidedObject)
    {
        if(gameObject.CompareTag("Player") && collidedObject.CompareTag("Obstacle"))
        {
            /*
            Vector2 ballMovement = (ball.transform.position - lastBallPosition).normalized;

            if(Rays.SendRay(ball.transform.position, ballMovement, Color.black, 10) != 0)
                pushers[gameObject].Score += 1;
            */
            pushers[gameObject].Score += 1;
        }
        else if (gameObject.CompareTag("Obstacle") && collidedObject.CompareTag("Goal"))
        {
            balls[gameObject].SetAllPlayersInactive();
            balls[gameObject].ClearObstacles();
            balls[gameObject].active = false;
        }
    }

}
