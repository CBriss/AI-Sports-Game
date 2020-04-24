using UnityEngine;

public class PlayerBoatGame : MonoBehaviour
{
    public IGameManager game;
    
    void Start()
    {
        Vector3 newPlayerNormalizedPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        newPlayerNormalizedPosition.z = 0;
        game = gameObject.GetComponent<IGameManager>();
        game.AddPlayer(newPlayerNormalizedPosition);
    }
}
