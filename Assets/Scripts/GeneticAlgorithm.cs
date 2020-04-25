using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithm : MonoBehaviour
{
    public int populationSize;
    public Player bestIndividual;
    public int generationCount;
    public IGameManager game;
    public GameObject UI;

    private GameObject InstanceUI;

    private void Start()
    {
        game = gameObject.GetComponent<IGameManager>();
        generationCount = 1;
        MakeGenerationZero();
        InstanceUI = Instantiate(UI);
        InstanceUI.GetComponentInChildren<Text>().text = "Generation: " + generationCount +
                "\n" + "Current Champion's Score: " + (bestIndividual != null ? bestIndividual.score.ToString() : "0");
    }
    void Update()
    {
        if (game.GetActivePlayers().Count <= 0)
        {
            NewGeneration(game.GetInactivePlayers());
            game.Clear();
            game.SetInactivePlayers(new List<Player>());
            generationCount += 1;
            InstanceUI.GetComponentInChildren<Text>().text = "Generation: " + generationCount +
                "\n" + "Current Champion's Score: " + (bestIndividual != null ? bestIndividual.score.ToString() : "0");

        }
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
            bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile();
        }

        // Combine parent genes
        // SKIPPING FOR NOW

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(
                new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.5f), 0)
            );
            newPlayerNormalizedPosition.z = 0;
            Player newPlayer = game.AddPlayer(newPlayerNormalizedPosition);
            newPlayer.playerObject.GetComponent<GameComponent>().brain =
                bestIndividual.playerObject.GetComponent<GameComponent>().brain.Clone();
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
