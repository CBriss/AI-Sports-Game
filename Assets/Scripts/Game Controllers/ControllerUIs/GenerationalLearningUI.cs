using UnityEngine;
using UnityEngine.UI;

public class GenerationalLearningUI : GameControllerUI
{
    [SerializeField]
    private GameObject UICanvas;
    public override void InitalizeUI()
    {
        UICanvas.SetActive(true);
        GameObject.Find("Save Best Panel").SetActive(false);
    }

    public override void UpdateUI(params string[] textValues)
    {
        gameObject.GetComponentInChildren<Text>().text = "Generation: " + textValues[0] +
                "\n" + "Current Champion's Score: " + textValues[1];
    }
}
