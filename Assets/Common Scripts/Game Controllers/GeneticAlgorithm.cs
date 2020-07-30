using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GeneticAlgorithm : GameController
{
    public int populationSize;
    public Player bestIndividual;
    public int generationCount;
    public float mutationPercentage;
    public float mutationAmount;

    public IGame game;
    public GameObject startMenuPrefab;
    public GameObject UIPrefab;
    public GamePieceTemplate playerTemplate;
    public GamePieceTemplate obstacleTemplate;
    public float gameTimer = -1;

    public static event Action<string[]> OnUpdateUI;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetPlayerTemplate(playerTemplate);
        game.SetObstacleTemplate(obstacleTemplate);

        if(gameTimer > 0)
            game.SetTimer(gameTimer);

        game.OnGameStart += StartGameController;
        game.OnGameOver += HandleGameOver;

        GameObject startMenu = Instantiate(startMenuPrefab);
        startMenu.transform.SetParent(gameObject.transform);
        Instantiate(UIPrefab);

        generationCount = 1;
    }

    public override void StartGameController()
    {
        if(generationCount == 1)
            MakeGenerationZero();

        StartCoroutine(game.StartAllSimulations());
        string[] UIString = { generationCount.ToString(), (bestIndividual != null ? bestIndividual.Score.ToString() : "0") };
        OnUpdateUI(UIString);
    }

    public override void HandleGameOver()
    {
        List<Player> previousGeneration = new List<Player>();
        foreach (Simulation simulation in game.GetSimulations(true, true).ToArray())
        {
            Player[] players = new Player[simulation.GetPlayers().Count];
            simulation.GetPlayers().CopyTo(players);
            previousGeneration.AddRange(players);
            simulation.Clear();
            game.RemoveSimulation(simulation);
        }

        generationCount += 1;
        NewGeneration(previousGeneration);
        string[] UIString = { generationCount.ToString(), (bestIndividual != null ? bestIndividual.Score.ToString() : "0") };
        OnUpdateUI(UIString);
    }

    public override GamePieceTemplate GetPlayerTemplate()
    {
        return playerTemplate;
    }

    public override GamePieceTemplate GetObstacleTemplate()
    {
        return obstacleTemplate;
    }

    public void MakeGenerationZero()
    {
        Debug.Log("Making Generation Zero");
        for (int i = 0; i < populationSize; i++)
        {
            Simulation simulation = game.AddSimulation();
            game.AddPlayer(simulation);
        }
    }

    void NewGeneration(List<Player> deadIndividuals)
    {
        for(int i=1; i<SceneManager.sceneCount; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        // Determine Generation Parents
        List<Player> sortedParents = deadIndividuals.OrderByDescending(p => p.Score).ToList();
        Debug.Log(sortedParents[0].Score);
        if (bestIndividual == null || sortedParents[0].Score > bestIndividual.Score)
        {
            if (bestIndividual != null)
                Destroy(bestIndividual.PlayerObject);
            bestIndividual = sortedParents[0];
            bestIndividual.simulation.RemoveFromInactivePlayers(bestIndividual);
        }

        //Replace bottom half of the population with the best genes and some mutation
        for (int i=0; i < populationSize; i++)
        {
            Simulation simulation = game.AddSimulation();
            NeuralNet newBrain = new NeuralNet(sortedParents[0].PlayerObject.GetComponent<GamePiece>().brain.networkShape);
            
            if (i >= populationSize / 2)
            {
                //bottom half get a breed of the two best
                newBrain =
                sortedParents[0].PlayerObject.GetComponent<GamePiece>().brain.Breed(
                    sortedParents[1].PlayerObject.GetComponent<GamePiece>().brain,
                    mutationPercentage,
                    mutationAmount
                );
            }
            else
            {
                //top half breed with each other
                newBrain =
                sortedParents[i].PlayerObject.GetComponent<GamePiece>().brain.Breed(
                    sortedParents[i + 1].PlayerObject.GetComponent<GamePiece>().brain,
                    mutationPercentage,
                    mutationAmount
                );
            }
            Player newPlayer = game.AddPlayer(simulation, newBrain);
        }
        game.StartGame();
    }

    private List<Player> DetermineBestParents(List<Player> deadIndividuals)
    {
        return new List<Player>{
                    FindBestIndividual(deadIndividuals.GetRange(0, deadIndividuals.Count/2)),
                    FindBestIndividual(deadIndividuals.GetRange(deadIndividuals.Count/2, deadIndividuals.Count-deadIndividuals.Count/2))
                };
    }

    private Player FindBestIndividual(List<Player> deadIndividuals)
    {
        Player currentBest = deadIndividuals[0];
        int currentHighScore = currentBest.Score;
        foreach (Player player in deadIndividuals)
        {
            int currentScore = player.Score;
            if (currentScore > currentHighScore)
            {
            currentBest = player;
            currentHighScore = currentScore;
            }
        }
        return currentBest;
    }
}
