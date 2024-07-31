using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public bool isFlipped = false;
    private Color flipedColor = Color.red;
    private Color nonFlippedColor = Color.blue;

    private bool isStatic = false;

    [SerializeField]
    private Renderer panColor;

    private void Awake()
    {
        panColor.material.color = nonFlippedColor;
    }

    public IEnumerator StaticlizePan(float maxTime)
    {
        isStatic = true;
        float activateTime = 0f;

        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isStatic = false;

    }

    public void Flip()
    {
        if (isStatic)
            return;

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
