using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(InternalFlip))]
    public bool isFlipped { get; set; } = false;
  
    private Color flippedColor = Color.red;
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
   
    private void InternalFlip()
    {
        if (isStatic)
            return;

        if (isFlipped)
        {
            panColor.material.color = flippedColor;
        }
        else
        {
            panColor.material.color = nonFlippedColor;
        }
    }

    public void Flip()
    {
        if (!HasStateAuthority)
        {
            if (!isFlipped)
            {
                panColor.material.color = flippedColor;
            }
            else
            {
                panColor.material.color = nonFlippedColor;
            }
            return;
        }

        isFlipped = !isFlipped;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcFlip()
    {
        if (!HasStateAuthority)
        {
            if (!isFlipped)
            {
                panColor.material.color = flippedColor;
            }
            else
            {
                panColor.material.color = nonFlippedColor;
            }
            return;
        }

        isFlipped = !isFlipped;
    }

}