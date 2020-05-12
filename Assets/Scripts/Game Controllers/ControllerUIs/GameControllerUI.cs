using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameControllerUI : MonoBehaviour
{
    public abstract void InitalizeUI();
    public abstract void UpdateUI(params string[] textValues);
}
