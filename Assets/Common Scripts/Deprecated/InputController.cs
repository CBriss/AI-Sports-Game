using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public List<KeyCode> possibleInputs;
    public List<KeyCode> currentInput;
    // Start is called before the first frame update
    void Start()
    {
        currentInput = new List<KeyCode>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyCode code in possibleInputs)
        {
            if (Input.GetKeyDown(code))
                currentInput.Add(code);
            if (Input.GetKeyUp(code))
                currentInput.Remove(code);
        }
    }
}
