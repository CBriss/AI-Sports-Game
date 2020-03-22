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

    // Start is called before the first frame update
    void Start()
    {
        generationCount = 1;
        population = new List<GameObject>();
        NewGeneration(this.population);
    }

    // Update is called once per frame
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
            this.population = new List<GameObject>();
            NewGeneration(this.population);
            generationCount += 1;
        }
        // Else
        // Sort by species
        // determine performance within species
        // Determine parents
        // Make children/mutated individuals
    }

    void NewGeneration(List<GameObject> population)
    {    
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(NewIndividual());
        }
}

    public GameObject NewIndividual()
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
        return individual;
    }

}
