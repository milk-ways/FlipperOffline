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

    public string CalcPan()
    {
        int redPanCount = 0;
        int bluePanCount = 0;
        string str = "";

        foreach (var pan in panGroup)
        {
            if (pan.isFlipped)
            {
                redPanCount++;
            }
            else
            {
                bluePanCount++;
            }
        }

        if (redPanCount > bluePanCount)
        {
            str = redPanCount.ToString() + " : " + bluePanCount.ToString() + "\nRED WIN";
        }
        else
        {
            str = redPanCount.ToString() + " : " + bluePanCount.ToString() + "\nBLUE WIN";
        }

        return str;
    }
}