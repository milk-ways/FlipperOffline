using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public VariableJoystick joystick;
    public Button Button;

    public void Inactivate()
    {
        joystick.InitializeJoystick();
        joystick.gameObject.SetActive(false);
        Button.interactable = false;
    }
}
