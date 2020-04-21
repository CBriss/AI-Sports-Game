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

    /*
    combineParentGenes(parentA, parentB, child)
    {
        let parentA_input_layer = parentA.brain.input_weights.dataSync();
        let parentA_output_layer = parentA.brain.output_weights.dataSync();
        let parentB_input_layer = parentB.brain.input_weights.dataSync();
        let parentB_output_layer = parentB.brain.output_weights.dataSync();

        let crossoverPoint = Math.floor(
          Math.random() * parentA_input_layer.length -
            parentA_input_layer.length / 2
        );
        let child_in_dna = [
          ...parentA_input_layer.slice(0, crossoverPoint),
          ...parentB_input_layer.slice(crossoverPoint, parentB_input_layer.length)
        ];
        let child_out_dna = [
          ...parentA_output_layer.slice(0, crossoverPoint),
          ...parentB_output_layer.slice(crossoverPoint, parentB_output_layer.length)
        ];

        let input_shape = child.brain.input_weights.shape;
        let output_shape = child.brain.output_weights.shape;
        child.brain.dispose();
        child.brain.input_weights = tf.tensor(child_in_dna, input_shape);
        child.brain.output_weights = tf.tensor(child_out_dna, output_shape);
        return child;
    }
    */
}
