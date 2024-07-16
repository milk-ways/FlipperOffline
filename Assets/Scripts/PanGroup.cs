using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanGroup : MonoBehaviour
{
    public static List<Pan> panGroup = new List<Pan>();

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddPan();
        }
    }*/

    private void Awake()
    {
        foreach (var pan in gameObject.transform.GetComponentsInChildren<Pan>())
        {
            panGroup.Add(pan);
        }
    }
}