using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCharacter : Character
{
    private bool isActivated = false;

    protected override void Action()
    {
        StartCoroutine(AcitvateTime(1f));

        StartCoroutine(CoolTime());
    }

    public IEnumerator AcitvateTime(float maxTime)
    {
        isActivated = true;
        float activateTime = 0f;

        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var pan = other.gameObject.GetComponent<Pan>();

        if (pan != null)
        {
            pan.Flip();
            if (isActivated)
                StartCoroutine(pan.StaticlizePan(3f));
        }
    }
}
