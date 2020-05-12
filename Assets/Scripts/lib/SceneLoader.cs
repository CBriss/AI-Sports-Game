using TMPro;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader SharedInstance;

    public enum Scenes
    {
        BoatGameScene,
        MainMenuScene,
        NeuralNetworkTest
    }

    public enum GameControllers
    {
        GeneticAlgorithm,
        SpawnChildren,
        BrainTest,
        PlayerGame
    }

    Scenes selectedScene;
    GameControllers selectedGameController;

    void Awake()
    {
        SharedInstance = this;
    }

    public void SetSelectedScene(string selectedScene)
    {
        this.selectedScene = (Scenes)System.Enum.Parse(typeof(Scenes), selectedScene);
        GameObject.Find("Game Scene Text").GetComponent<TextMeshProUGUI>().text = selectedScene;
    }

    public void SetSelectedGameController(string selectedGameController)
    {
        this.selectedGameController = (GameControllers)System.Enum.Parse(typeof(GameControllers), selectedGameController);
        GameObject.Find("Game Controller Text").GetComponent<TextMeshProUGUI>().text = selectedGameController;
    }

    public void Load()
    {
        if(selectedScene.ToString().Contains("Game"))
            Loader.Load(selectedScene.ToString(), selectedGameController.ToString());
        else
            Loader.Load(selectedScene.ToString());
    }

    public void LoadMenu()
    {
        Loader.Load(Scenes.MainMenuScene.ToString());
    }
}
