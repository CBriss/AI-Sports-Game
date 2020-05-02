using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public void LoadMenu()
    {
        Loader.Load(Loader.Scenes.MainMenuScene.ToString());
    }
    public void LoadNeuralNetGame()
    {
        Loader.Load(Loader.Scenes.NeuralNetworkTest.ToString());
    }

    public void LoadBoatGame(string gameController)
    {
        Loader.Load(
            Loader.Scenes.BoatGameScene.ToString(),
            (Loader.GameControllers)System.Enum.Parse(typeof(Loader.GameControllers), gameController)
        );
    }

}
