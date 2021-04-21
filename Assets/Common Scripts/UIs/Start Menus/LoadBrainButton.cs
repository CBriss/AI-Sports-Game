using System;
using TMPro;
using UnityEngine;

public class LoadBrainButton : MonoBehaviour
{
    public static event Action<string> OnBrainSelected;

    public void BrainSelected()
    {
        OnBrainSelected.Invoke(gameObject.GetComponent<TextMeshProUGUI>().text);
    }
}
