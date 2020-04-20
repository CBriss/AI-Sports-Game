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
            MakeGenerationZero();
            // NewGeneration();
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

    /*

    void NewGeneration()
    {
        // Determine Generation Parents
        List<GameObject> parents = DetermineBestParents();

        // Combine parent genes
        // SKIPPING FOR NOW

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            GameObject individual = NewIndividual();
            individual.GetComponent<Boat>().brain = parents[0].GetComponent<Boat>().brain.Clone();
            this.population.Add(individual);
        }
        this.population = new List<GameObject>();
    }

      private List<GameObject> DetermineBestParents()
      {
        return new List<GameObject>{
                FindBestIndividual(),
                this.population[ UnityEngine.Random.Range(0, this.population.Count)]
            };
      }

    private GameObject FindBestIndividual(){
        GameObject currentBest = this.population[0];
        int currentHighScore = currentBest.GetComponent<Boat>().score;
        foreach (GameObject individual in this.population)
        {
            int currentScore = individual.GetComponent<Boat>().score;
            if(currentScore > currentHighScore){
                currentBest = individual;
                currentHighScore = currentScore;
            }
        }
        return currentBest;
    }

    */
}
