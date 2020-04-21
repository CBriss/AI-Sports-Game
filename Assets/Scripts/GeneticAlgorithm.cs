using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public int populationSize;
    public GameObject bestIndividual;
    public int generationCount;
    public IGameManager game;

    private void Start()
    {
        game = gameObject.GetComponent<IGameManager>();
        generationCount = 1;
        MakeGenerationZero();
    }
    void Update()
    {
        if (game.GetActivePlayers().Count <= 0)
        {
            NewGeneration(game.GetInactivePlayers());
            foreach(Player player in game.GetInactivePlayers())
            {
                Destroy(player.playerObject);
            }
            game.SetInactivePlayers(new List<Player>());
            generationCount += 1;

        }
    }

    void MakeGenerationZero()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            game.AddPlayer(newPlayerNormalizedPosition);
        }
    }

    void NewGeneration(List<Player> deadIndividuals)
    {
        // Determine Generation Parents
        List<Player> parents = DetermineBestParents(deadIndividuals);

        // Combine parent genes
        // SKIPPING FOR NOW

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            Player newPlayer = game.AddPlayer();
            newPlayer.playerObject.GetComponent<GameComponent>().brain = 
                parents[0].playerObject.GetComponent<GameComponent>().brain.Clone();
        }
    }

      private List<Player> DetermineBestParents(List<Player> deadIndividuals)
      {
        return new List<Player>{
                FindBestIndividual(deadIndividuals),
                deadIndividuals[ Random.Range(0, deadIndividuals.Count)]
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
