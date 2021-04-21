using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerationalLearningUI : GameControllerUI
{
    [SerializeField]
    private GameObject UICanvas;
    [SerializeField]
    private GameObject savePanel;
    [SerializeField]
    private GameObject saveFileInput;
    public override void Start()
    {
        UICanvas.SetActive(true);
        savePanel.SetActive(false);
        GeneticAlgorithm.OnUpdateUI += UpdateUI;
    }

    public void OnDestroy()
    {
        GeneticAlgorithm.OnUpdateUI -= UpdateUI;
    }

    public override void UpdateUI(params string[] textValues)
    {
        gameObject.GetComponentInChildren<Text>().text = "Generation: " + textValues[0] +
                "\n" + "Current Champion's Score: " + textValues[1];
    }

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
    }

    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
    }

    public void SaveBoat()
    {
        string playerName = saveFileInput.GetComponent<TMP_InputField>().text;
        GeneticAlgorithm geneticAlgorithm = FindObjectOfType<GeneticAlgorithm>();
        GameObject playerObject = geneticAlgorithm.bestIndividual.PlayerObject;
        playerObject.GetComponent<GamePiece>().brain.SaveToFile("Assets/Saved Brains/"+playerName+".txt");
        // bestIndividual.playerObject.GetComponent<GameComponent>().brain.SaveToFile("Assets/Saved Brains/bestBoat.txt");
        Debug.Log("Saved!");
        Debug.Log("Look in Saved Brains/"+playerName);
    }
}
