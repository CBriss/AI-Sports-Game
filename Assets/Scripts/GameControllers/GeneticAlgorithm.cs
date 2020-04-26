using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : GameController
{
    public int populationSize;
    public Player bestIndividual;
    public int generationCount;
    public IGame game;
    public GameObject UI;
    private GameObject InstanceUI;
    public bool useSeedBrain;
    public float mutationPercentage;
    public float mutationAmount;

    public override void Start()
    {
        game = gameObject.GetComponent<IGame>();
        generationCount = 1;
        MakeGenerationZero();
        InstanceUI = Instantiate(UI);
        InstanceUI.GetComponentInChildren<Text>().text = "Generation: " + generationCount +
                "\n" + "Current Champion's Score: " + (bestIndividual != null ? bestIndividual.score.ToString() : "0");
    }
    public override void Update()
    {
        if (game.GetActivePlayers().Count <= 0)
        {
            NewGeneration(game.GetInactivePlayers());
            foreach(Player player in game.GetInactivePlayers())
            {
                if(player != bestIndividual)
                    Destroy(player.playerObject);
            }
            game.ClearInactivePlayers();
            generationCount += 1;
            InstanceUI.GetComponentInChildren<Text>().text = "Generation: " + generationCount +
                "\n" + "Current Champion's Score: " + (bestIndividual != null ? bestIndividual.score.ToString() : "0");
        }
    }

    void MakeGenerationZero()
    {
        NeuralNet seedBrain;
        if (useSeedBrain)
            seedBrain = new NeuralNet("Assets/Scripts/bestBoat.txt");
        else
            seedBrain = new NeuralNet(new int[3]);
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            if (useSeedBrain)
                game.AddPlayer(newPlayerNormalizedPosition);
            else
                game.AddPlayer(newPlayerNormalizedPosition, seedBrain);
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
            bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile("Assets/Scripts/bestBoat.txt");
        }

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            Player newPlayer = game.AddPlayer(newPlayerNormalizedPosition);
            newPlayer.playerObject.GetComponent<GameComponent>().brain =
                bestIndividual.playerObject.GetComponent<GameComponent>().brain.Breed(
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
