using UnityEngine.UI;

public class PlayerGameUI : GameControllerUI
{
    public override void Start()
    {
        PlayerGame.OnUpdateUI += UpdateUI;
    }

    public override void UpdateUI(params string[] textValues)
    {
        gameObject.GetComponentInChildren<Text>().text = "Score: " + textValues[0];
    }

    public void OnDestroy()
    {
        PlayerGame.OnUpdateUI -= UpdateUI;
    }
}
