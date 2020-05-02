using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public enum Scenes
    {
        BoatGameScene,
        MainMenuScene,
        NeuralNetworkTest
    }

    public void LoadBoatGame(string gameController)
    {
        Loader.Load(Scenes.BoatGameScene.ToString(), gameController);
    }
}
