using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public int populationSize;
    public GameObject indivualPrefab;
    public List<GameObject> population;
    public GameObject bestIndividual;
    public int generationCount;

    void Start()
    {
        generationCount = 1;
        population = new List<GameObject>();
        NewGeneration(this.population);
    }

    void Update()
    {
        int activeObjects = 0;
        for(int i=0; i < this.populationSize; i++)
        {
            if (population[i].activeInHierarchy)
                activeObjects += 1;
        }

        if(activeObjects <= 0)
        {
            NewGeneration(this.population);
            generationCount += 1;
        }
    }

    void MakeGenerationZero(){
        for (int i = 0; i < populationSize; i++)
        {
            population.Add( NewIndividual(parents[0].GetComponent<Boat>().brain.Clone()) );
        }
    }
    void NewGeneration(List<GameObject> population)
    {    
        // Determine Generation Parents
        List<GameObject> parents = DetermineBestParents();

        // Combine parent genes
        // SKIPPING FOR NOW

        // Create new population with those genes and a mutation chance
        for (int i = 0; i < populationSize; i++)
        {
            population.Add( NewIndividual(parents[0].GetComponent<Boat>().brain.Clone()) );
        }
        this.population = new List<GameObject>();
    }

    private List<GameObject> DetermineBestParents(){
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

    public GameObject NewIndividual(NeuralNet brain)
    {
        Vector3 newObstacleNormalizedPosition = Camera.main.ViewportToWorldPoint(
            new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)
        );
        newObstacleNormalizedPosition.z = 0;

        GameObject individual = ObjectPooling.SharedInstance.GetPooledObject("Boat");
        if (individual != null)
        {
            individual.transform.position = newObstacleNormalizedPosition;
            individual.SetActive(true);
        }

        individual.GetComponent<Boat>().brain = brain;

        return individual;
    }

}
