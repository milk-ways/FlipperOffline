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

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        joystick.InitializeJoystick();
        joystick.gameObject.SetActive(false);
#endif
#if UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif

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
