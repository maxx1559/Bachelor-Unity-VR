using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenu : MonoBehaviour
{
    public GameObject wristUI;
    public Canvas wristCanvas;
    public bool activeWristUI = true;
    // Start is called before the first frame update
    void Start()
    {
        DisplayWristUI();
    }

    public void backToOptions()
    {
        Debug.Log("Clicked BackToOptions");
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }

    public void DisplayWristUI()
    {
        if (activeWristUI)
        {
            wristCanvas.enabled = activeWristUI;
            //wristUI.SetActive(true);
            activeWristUI = false;
        } else if (!activeWristUI)
        {
            wristCanvas.enabled = activeWristUI;
            //wristUI.SetActive(false);
            activeWristUI = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
