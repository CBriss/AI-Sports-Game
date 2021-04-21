using UnityEngine;

public abstract class GameControllerUI : MonoBehaviour
{
    public abstract void Start();
    public abstract void UpdateUI(params string[] textValues);
}
