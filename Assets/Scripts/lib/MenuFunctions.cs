using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{

  public void LoadMenu()
  {
    SceneManager.LoadScene("MenuScene");
  }

  public void LoadBoatGame()
  {
    SceneManager.LoadScene("BoatGameScene");
  }

  public void LoadNeuralNetworkGame()
  {
    SceneManager.LoadScene("NeuralNetworkTest");
  }
}
