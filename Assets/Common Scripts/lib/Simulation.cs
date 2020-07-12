using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation
{
    public List<Player> activePlayers = new List<Player>();
    public List<Player> inactivePlayers = new List<Player>();
    public List<GameObject> obstacles = new List<GameObject>();

    public int playerCount;

    public bool active = true;

    /****************
    * Adding Pieces *
    *****************/
    public void AddPlayer(Player player)
    {
        activePlayers.Add(player);
        playerCount++;
    }

    public void AddObstacle(GameObject obstacle)
    {
        obstacles.Add(obstacle);
    }

    /****************
    * Moving Pieces *
    ****************/
    public void ActivatePlayer(Player player)
    {
        player.PlayerObject.SetActive(true);
        activePlayers.Remove(player);
        inactivePlayers.Add(player);
    }

    public void DeactivatePlayer(Player player)
    {
        player.PlayerObject.SetActive(false);
        activePlayers.Remove(player);
        inactivePlayers.Add(player);
    }

    public void SetAllPlayersInactive()
    {
        foreach (Player player in activePlayers.ToArray())
        {
            DeactivatePlayer(player);
        }
    }

    /*************
    * Get Pieces *
    *************/
    public List<Player> GetPlayers()
    {
        return activePlayers.Concat(inactivePlayers).ToList();
    }
    
    public List<Player> GetActivePlayers()
    {
        return activePlayers;
    }

    public List<Player> GetInactivePlayers()
    {
        return inactivePlayers;
    }

    public List<GameObject> GetObstacles()
    {
        return obstacles;
    }

    /******************
    * Removing Pieces *
    ******************/
    public void Clear()
    {
        ClearActivePlayers();
        ClearInactivePlayers();
        ClearObstacles();
    }

    public void ClearActivePlayers()
    {
        foreach (Player player in activePlayers)
        {
            Object.Destroy(player.PlayerObject);
            playerCount--;
        }
        activePlayers.Clear();
    }

    public void ClearInactivePlayers()
    {
        foreach (Player player in inactivePlayers)
        {
            Object.Destroy(player.PlayerObject);
            playerCount--;
        }
        inactivePlayers.Clear();
    }

    public void RemoveFromActivePlayers(Player player)
    {
        activePlayers.Remove(player);
        playerCount--;
    }

    public void RemoveFromInactivePlayers(Player player)
    {
        inactivePlayers.Remove(player);
        playerCount--;
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            Object.Destroy(obstacle);
        }
        obstacles.Clear();
    }


    /*******
    * Misc *
    *******/
    public void ClearObstacles(GameObject obstacle)
    {
        obstacles.Remove(obstacle);
    }

    public void IgnoreCollisionsWith(GameObject objectToIgnore)
    {
        foreach (Player player in activePlayers)
        {
            Physics2D.IgnoreCollision(objectToIgnore.gameObject.GetComponent<Collider2D>(), player.PlayerObject.GetComponent<Collider2D>());
        }

        foreach (GameObject obstacle in obstacles)
        {
            Physics2D.IgnoreCollision(objectToIgnore.gameObject.GetComponent<Collider2D>(), obstacle.GetComponent<Collider2D>());
        }

    }

    public bool CheckIfActive()
    {
        if (activePlayers.Count > 0)
            return false;

        active = false;
        return true;
    }
}
