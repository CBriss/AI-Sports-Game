using UnityEngine;

public class Player
{
    public GameObject playerObject { get; }
    public int score { get; set; }

    public Player(GameObject playerGameObject)
    {
        playerObject = playerGameObject;
        score = 0;
    }
}
