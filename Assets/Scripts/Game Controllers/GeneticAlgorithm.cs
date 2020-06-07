using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static event Action<string[]> OnUpdateUI;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetPlayerTemplate(playerTemplate);
        game.SetObstacleTemplate(obstacleTemplate);

        game.OnGameStart += StartGameController;
        game.OnGameOver += HandleGameOver;

        GameObject startMenu = Instantiate(startMenuPrefab);
        startMenu.transform.SetParent(gameObject.transform);
        Instantiate(UIPrefab);
    }

    public override void StartGameController()
    {
        generationCount = 1;
        MakeGenerationZero();
        string[] UIString = { generationCount.ToString(), (bestIndividual != null ? bestIndividual.score.ToString() : "0") };
        OnUpdateUI(UIString);
    }

    public override void HandleGameOver()
    {
        game.ClearActivePlayers();
        game.ClearObstacles();
        NewGeneration(game.GetInactivePlayers());
        game.ClearInactivePlayers();
        generationCount += 1;
        string[] UIString = { generationCount.ToString(), (bestIndividual != null ? bestIndividual.score.ToString() : "0") };
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
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                    new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            game.AddPlayer(newPlayerNormalizedPosition);
        }
    }

    void NewGeneration(List<Player> deadIndividuals)
    {
        // Determine Generation Parents
        //List<Player> parents = DetermineBestParents(deadIndividuals);

        List<Player> sortedParents = deadIndividuals.OrderByDescending(p => p.score).ToList();
        Debug.Log(sortedParents[0].score);
        if (bestIndividual == null || sortedParents[0].score > bestIndividual.score)
        {
            if (bestIndividual != null)
            Destroy(bestIndividual.playerObject);
            bestIndividual = sortedParents[0];
            game.RemoveFromInactivePlayers(bestIndividual);
        }
        //Replace bottom half of the population with the best genes and some mutation
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                    new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            Player newPlayer = game.AddPlayer(newPlayerNormalizedPosition);
            //bottom half get a breed of the two best
            if (i >= populationSize / 2)
            {
            newPlayer.playerObject.GetComponent<GamePiece>().brain =
                sortedParents[0].playerObject.GetComponent<GamePiece>().brain.Breed(
                sortedParents[1].playerObject.GetComponent<GamePiece>().brain,
                mutationPercentage,
                mutationAmount
            );
            }
            else
            {
            //top half breed with each other
            newPlayer.playerObject.GetComponent<GamePiece>().brain =
                sortedParents[i].playerObject.GetComponent<GamePiece>().brain.Breed(
                sortedParents[i + 1].playerObject.GetComponent<GamePiece>().brain,
                mutationPercentage,
                mutationAmount
            );
            }

        }
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
        int currentHighScore = currentBest.score;
        foreach (Player player in deadIndividuals)
        {
            int currentScore = player.score;
            if (currentScore > currentHighScore)
            {
            currentBest = player;
            currentHighScore = currentScore;
            }
        }
        return currentBest;
    }
}
