using System.Collections.Generic;
using UnityEngine;

public class GameBase : MonoBehaviour
{
    public List<Simulation> simulations = new List<Simulation>();
    public bool active = false;

    public GameObject playerContainer;
    public GamePieceTemplate playerTemplate;
    public int playerLayer;

    public GameObject obstacleContainer;
    public GamePieceTemplate obstacleTemplate;
    public int obstacleLayer;

    /*********************
    * Simulation Methods *
    *********************/
    public Simulation AddSimulation()
    {
        Simulation newSimulation = new Simulation();
        simulations.Add(newSimulation);
        return newSimulation;
    }

    public void RemoveSimulation(Simulation simulation)
    {
        simulation.Clear();
        simulations.Remove(simulation);
    }

    public List<Simulation> GetSimulations(bool includeActive, bool includeInactive)
    {
        List<Simulation> simuulationList = new List<Simulation>();
        foreach (Simulation simulation in simulations)
        {
            if (simulation.active && includeActive)
                simuulationList.Add(simulation);
            else if (!simulation.active && includeInactive)
                simuulationList.Add(simulation);
        }
        return simuulationList;
    }

    public void ClearSimulations(bool includeActive, bool includeInactive)
    {
        List<Simulation> simuulationList = new List<Simulation>();
        foreach (Simulation simulation in simulations)
        {
            if (simulation.active && includeActive)
            {
                simulation.Clear();
                simuulationList.Remove(simulation);
            }
            else if (!simulation.active && includeInactive)
            {
                simulation.Clear();
                simuulationList.Remove(simulation);
            }
        }
    }

    /*****************
    * Player Methods *
    *****************/
    public void SetPlayerTemplate(GamePieceTemplate playerTemplate)
    {
        this.playerTemplate = playerTemplate;
    }

    /*******************
    * Obstacle Methods *
    *******************/
    public void SetObstacleTemplate(GamePieceTemplate obstacleTemplate)
    {
        this.obstacleTemplate = obstacleTemplate;
    }

    /*******************
    *       Misc       *
    *******************/
    public void Clear()
    {
        ClearSimulations(true, true);
    }

    public bool IsActive()
    {
        return active;
    }

}

