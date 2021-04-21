using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadBrainStart : MonoBehaviour
{
    [SerializeField]
    private GameObject viewportContent;

    [SerializeField]
    private GameObject brainSelectButtonPrefab;

    public void Awake()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Saved Brains");
        FileInfo[] info = dir.GetFiles("*.txt");

        Vector3 offsetPos = new Vector3(0.0f, -20.0f, 0.0f);

        foreach (FileInfo f in info)
        {
            string fileString = f.ToString().Split('\\').Last();
            GameObject newButton = Instantiate(brainSelectButtonPrefab);
            newButton.transform.position = viewportContent.transform.position;
            RectTransform rect = newButton.GetComponent<RectTransform>();
            rect.SetParent(viewportContent.transform);
            rect.position = rect.position + offsetPos;

            offsetPos.y -= 30.0f;

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileString;
        }

        LoadBrainButton.OnBrainSelected += HideMenu;
    }

    public void OnDestroy()
    {
        LoadBrainButton.OnBrainSelected -= HideMenu;
    }

    public void HideMenu(string brainName)
    {
        gameObject.SetActive(false);
    }
}
