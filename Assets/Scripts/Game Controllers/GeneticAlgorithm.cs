using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : GameController
{
    public int populationSize;
    public Player bestIndividual;
    public int generationCount;
    public IGame game;
    public GameControllerUI templateUI;
    public GameComponentTemplate playerTemplate;
    public GameComponentTemplate obstacleTemplate;
    public float mutationPercentage;
    public float mutationAmount;

    public void Start()
    {
        game = gameObject.GetComponent<IGame>();
        game.SetGameController(this);

        game.OnGameStart += StartGame;
        game.OnGameOver += RestartGame;
    }

    public override void StartGame()
    {
        generationCount = 1;
        MakeGenerationZero();
        templateUI.InitalizeUI();
        templateUI.UpdateUI(generationCount.ToString(), (bestIndividual != null ? bestIndividual.score.ToString() : "0"));
    }

    public override void Update() {}

    public void RestartGame()
    {
        game.ClearActivePlayers();
        game.ClearObstacles();
        NewGeneration(game.GetInactivePlayers());
        game.ClearInactivePlayers();
        generationCount += 1;
        templateUI.UpdateUI(generationCount.ToString(), (bestIndividual != null ? bestIndividual.score.ToString() : "0"));
    }

    public override GameComponentTemplate GetPlayerTemplate()
    {
        return playerTemplate;
    }

    public override GameComponentTemplate GetObstacleTemplate()
    {
        return obstacleTemplate;
    }

    void MakeGenerationZero()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f), 0)
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
            bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile("Assets/Saved Brains/bestBoat.txt");
        }

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f), 0)
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

    public void OpenSaveMenu()
    {
        GameObject.Find("Save Best Panel").SetActive(true);
        GameObject.Find("Best Boat Score Text").GetComponent<Text>().text = bestIndividual.score.ToString();
    }

    public void SaveToFile()
    {
        bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile(GameObject.Find("Best Boat Name Input").GetComponent<Text>().text);
    }
}
