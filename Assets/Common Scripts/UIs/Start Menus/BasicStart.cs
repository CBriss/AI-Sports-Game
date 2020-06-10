using UnityEngine;

public class BasicStart : MonoBehaviour
{
    public void StartGame()
    {
        GetComponentInParent<IGame>().StartGame();
        gameObject.SetActive(false);
    }    
}
