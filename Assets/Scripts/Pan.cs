using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public bool isFlipped = false;
    private Color flipedColor = Color.red;
    private Color nonFlippedColor = Color.blue;

    [SerializeField]
    private Renderer panColor;

    private void Awake()
    {
        panColor.material.color = nonFlippedColor;
    }

    public void Flip()
    {
        isFlipped = !isFlipped;

        if (isFlipped)
        {
            panColor.material.color = flipedColor;
        }
        else
        {
            panColor.material.color = nonFlippedColor;
        }
    }
}
