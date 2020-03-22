using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{

  public void LoadMenu()
  {
    Debug.Log("Menu leggo");
    SceneManager.LoadScene("MenuScene");
  }

  public void LoadBoatGame()
  {
    Debug.Log("Boat Game leggo");
    SceneManager.LoadScene("BoatGameScene");
  }

  public void LoadNeuralNetworkGame()
  {
    Debug.Log("NN Game leggo");
    SceneManager.LoadScene("NeuralNetworkTest");
  }
}
