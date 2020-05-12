using UnityEngine;
using UnityEngine.UI;

public class GenerationalLearningUI : GameControllerUI
{
    public override void InitalizeUI()
    {
        gameObject.SetActive(true);
        GameObject.Find("Save Best Panel").SetActive(false);
    }

    public override void UpdateUI(params string[] textValues)
    {
        gameObject.GetComponentInChildren<Text>().text = "Generation: " + textValues[0] +
                "\n" + "Current Champion's Score: " + textValues[1];
    }
}
