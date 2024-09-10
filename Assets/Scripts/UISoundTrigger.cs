using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundTrigger : MonoBehaviour
{
    public void Awake()
    {
        var btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                SoundManager.PlayEffect("buttonClick");
            });
        }
    }
}
