using UnityEngine;

public class Player
{
    public GameObject PlayerObject { get; }
    public int Score { get; set; }

    public Simulation simulation;

    public Player(GameObject playerObject, Simulation simulation)
    {
        PlayerObject = playerObject;
        this.simulation = simulation;
        Score = 0;
    }


}
