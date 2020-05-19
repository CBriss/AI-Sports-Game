using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : GameController
{
    public int populationSize;
    public Player bestIndividual;
    public int generationCount;
    public float mutationPercentage;
    public float mutationAmount;

    public IGame game;
    public GameObject UIPrefab;
    public GameComponentTemplate playerTemplate;
    public GameComponentTemplate obstacleTemplate;

    public static event Action<string[]> OnUpdateUI;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetPlayerTemplate(playerTemplate);
        game.SetObstacleTemplate(obstacleTemplate);

        game.OnGameStart += StartGameController;
        game.OnGameOver += HandleGameOver;
        
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

    public override GameComponentTemplate GetPlayerTemplate()
    {
        return playerTemplate;
    }

    public override GameComponentTemplate GetObstacleTemplate()
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
        List<Player> parents = DetermineBestParents(deadIndividuals);

        if (bestIndividual == null || parents[0].score > bestIndividual.score)
        {
            if(bestIndividual != null)
                Destroy(bestIndividual.playerObject);
            bestIndividual = parents[0];
            game.RemoveFromInactivePlayers(bestIndividual);
            // bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile("Assets/Saved Brains/bestBoat.txt");
        }

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            Player newPlayer = game.AddPlayer(newPlayerNormalizedPosition);
            newPlayer.playerObject.GetComponent<GameComponent>().brain =
                parents[0].playerObject.GetComponent<GameComponent>().brain.Breed(
                    parents[1].playerObject.GetComponent<GameComponent>().brain,
                    mutationPercentage,
                    mutationAmount
                );
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
            if(currentScore > currentHighScore){
                currentBest = player;
                currentHighScore = currentScore;
            }
        }
        return currentBest;
    }
}
