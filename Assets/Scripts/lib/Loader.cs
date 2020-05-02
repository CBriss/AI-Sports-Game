using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scenes
    {
        BoatGameScene,
        MainMenuScene,
        NeuralNetworkTest
    }

    public enum GameControllers
    {
        GeneticAlgorithm,
        PlayerGame
    }

    private static Action onLoaderCallback;


    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Load(string sceneName, GameControllers gameController)
    {
        onLoaderCallback = () =>
        {
            if (sceneName.ToString().Contains("Game"))
            {
                Scene scene = SceneManager.GetSceneByName(sceneName);
                LoadGameScene(scene, gameController);
            }
        };

        SceneManager.LoadScene(sceneName.ToString());
    }

    public static void LoaderCallback()
    {
        onLoaderCallback?.Invoke();
        onLoaderCallback = null;
    }

    private static void LoadGameScene(Scene scene, GameControllers gameController)
    {
        GameObject[] sceneObjects = scene.GetRootGameObjects();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == "GameWrapper")
            {
                (sceneObject.GetComponent(gameController.ToString()) as MonoBehaviour).enabled = true;
            }

        }
    }
}
