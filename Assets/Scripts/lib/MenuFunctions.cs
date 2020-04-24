using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{

  public void LoadMenu()
  {
    SceneManager.LoadScene("MenuScene");
  }

  public void LoadBoatGamePlayer()
  {
    SceneManager.LoadScene("BoatGamePlayerScene");
  }

    public void LoadBoatGameAI()
    {
        SceneManager.LoadScene("BoatGameAIScene");
    }

    public void LoadNeuralNetworkGame()
  {
    SceneManager.LoadScene("NeuralNetworkTest");
  }
}
