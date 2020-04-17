using UnityEngine;

public class Obstacle : GameComponent
{
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}
