using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
            game.ClearActivePlayers();
            game.ClearObstacles();
            NewGeneration(game.GetInactivePlayers());
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
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 0)
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
        //List<Player> parents = DetermineBestParents(deadIndividuals);
        
        List<Player> sortedParents = deadIndividuals.OrderByDescending(p=>p.score).ToList();
        Debug.Log(sortedParents[0].score);
        if (bestIndividual == null || sortedParents[0].score > bestIndividual.score)
        {
            if(bestIndividual != null)
                Destroy(bestIndividual.playerObject);
            bestIndividual = sortedParents[0];
            bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile("Assets/Scripts/bestBoat.txt");
        }
        //Replace bottom half of the population with the best genes and some mutation
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            Player newPlayer = game.AddPlayer(newPlayerNormalizedPosition);
            //bottom half get a breed of the two best
            if(i >= populationSize/2){
                newPlayer.playerObject.GetComponent<GameComponent>().brain =
                    sortedParents[0].playerObject.GetComponent<GameComponent>().brain.Breed(
                    sortedParents[1].playerObject.GetComponent<GameComponent>().brain,
                    mutationPercentage,
                    mutationAmount
                );
            }else{
                //top half breed with each other
                newPlayer.playerObject.GetComponent<GameComponent>().brain =
                    sortedParents[i].playerObject.GetComponent<GameComponent>().brain.Breed(
                    sortedParents[i+1].playerObject.GetComponent<GameComponent>().brain,
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
            if(currentScore > currentHighScore){
                currentBest = player;
                currentHighScore = currentScore;
            }
        }
        return currentBest;
    }
}
