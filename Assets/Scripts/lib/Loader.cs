using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Action onLoaderCallback;

    public static void Load(string sceneName, string gameController)
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

    private static void LoadGameScene(Scene scene, string gameController)
    {
        GameObject[] sceneObjects = scene.GetRootGameObjects();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == "GameWrapper")
            {
                Debug.Log($"Trying to enable {gameController}");
                (sceneObject.GetComponent(gameController) as MonoBehaviour).enabled = true;
            }

        }
    }
}
